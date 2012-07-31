using System;
using System.Linq;
using System.Configuration;
using System.Collections.Generic;
using HTA.Adventures.Models.Types;
using Nest;
using ServiceStack.Configuration;
using ServiceStack.OrmLite;
using ServiceStack.OrmLite.SqlServer;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceInterface.Auth;
using ServiceStack.ServiceInterface.ServiceModel;
using ServiceStack.WebHost.Endpoints;

namespace HTA.Adventures.API.WebService
{
    //Request DTO
    public class Hello
    {
        public string Name { get; set; }
    }

    //Response DTO
    public class HelloResponse : IHasResponseStatus
    {
        public string Result { get; set; }
        public ResponseStatus ResponseStatus { get; set; } //Where Exceptions get auto-serialized
    }

    //Can be called via any endpoint or format, see: http://servicestack.net/ServiceStack.Hello/
    public class HelloService : ServiceBase<Hello>
    {
        protected override object Run(Hello request)
        {
            return new HelloResponse { Result = "Hello, " + request.Name };
        }
    }


    public class NearByLocationsSearch
    {
        public List<AdventureLocation> Result { get; set; }
        public string LatLon { get; set; }
        public double Lat { get; set; }
        public double Lon { get; set; }
        public string Range { get; set; }
        public string Name { get; set; }

        public double[] SplitLatLon()
        {

            var split = LatLon.Split(',');
            double[] output = new double[split.Count()];
            for(int i = 0; i < split.Count(); i++)
            {
                output[i] = double.Parse(split[i]);
            }
            return output;
        }

        public void ValidateRange(string defaultRange)
        {
            if(string.IsNullOrEmpty(Range) || (!Range.EndsWith("mi") && !Range.EndsWith("km")))
            {
                Range = defaultRange;
            }
        }
    }

    public class NearBySearchResponse : IHasResponseStatus
    {
        public NearByLocationsSearch Request { get; set; }

        public NearBySearchResponse(NearByLocationsSearch request)
        {
            Request = request;
        }

        public List<AdventureLocation> Result { get; set; }
        public ResponseStatus ResponseStatus { get; set; }

    }

    public class NearByLocationSearch : RestServiceBase<NearByLocationsSearch>
    {

        public override object OnGet(NearByLocationsSearch request)
        {
            if(!string.IsNullOrEmpty(request.LatLon))
            {
                var values = request.SplitLatLon();
                request.Lat = values[0];
                request.Lon = values[1];
            }

            var setting = new ConnectionSettings("office.mtctickets.com", 9200);
            setting.SetDefaultIndex("pins");
            var client = new ElasticClient(setting);

            string DefaultRangeSetting = "15mi";
            request.ValidateRange(DefaultRangeSetting);

            var results = client.Search<AdventureLocation>(s => s
                .From(0)
                .Size(10)
                .Filter(f => f
                .GeoDistance("geo.location", filter => filter
                .Location(request.Lat, request.Lon)
                .Distance(request.Range)))
                .Index("pins")
                .Type("region")
               );

            var response = new NearBySearchResponse(request) {Result = results.Documents.ToList()};
            


            return response;
        }
    }

    //REST DTO
    public class Todo
    {
        public long Id { get; set; }
        public string Content { get; set; }
        public int Order { get; set; }
        public bool Done { get; set; }
    }

    //Todo REST Service implementation
    public class TodoService : RestServiceBase<Todo>
    {
        public TodoRepository Repository { get; set; }  //Injected by IOC

        public override object OnGet(Todo request)
        {
            if(request.Id == default(long))
                return Repository.GetAll();

            return Repository.GetById(request.Id);
        }

        public override object OnPost(Todo todo)
        {
            return Repository.Store(todo);
        }

        public override object OnPut(Todo todo)
        {
            return Repository.Store(todo);
        }

        public override object OnDelete(Todo request)
        {
            Repository.DeleteById(request.Id);
            return null;
        }
    }


    /// <summary>
    /// In-memory repository, so we can run the TODO app without any external dependencies
    /// Registered in Funq as a singleton, auto injected on every request
    /// </summary>
    public class TodoRepository
    {
        private readonly List<Todo> todos = new List<Todo>();

        public List<Todo> GetAll()
        {
            return todos;
        }

        public Todo GetById(long id)
        {
            return todos.FirstOrDefault(x => x.Id == id);
        }

        public Todo Store(Todo todo)
        {
            if(todo.Id == default(long))
            {
                todo.Id = todos.Count == 0 ? 1 : todos.Max(x => x.Id) + 1;
            }
            else
            {
                for(var i = 0; i < todos.Count; i++)
                {
                    if(todos[i].Id != todo.Id)
                        continue;

                    todos[i] = todo;
                    return todo;
                }
            }

            todos.Add(todo);
            return todo;
        }

        public void DeleteById(long id)
        {
            todos.RemoveAll(x => x.Id == id);
        }
    }
}
