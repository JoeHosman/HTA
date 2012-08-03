using System.Collections.Generic;
using System.Linq;
using HTA.Adventures.Models;
using HTA.Adventures.Models.Types;
using Nest;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceInterface.ServiceModel;
using System;

namespace HTA.Adventures.API.ServiceInterface
{

    public class AdvenuterTypeService : RestServiceBase<AdventureType>
    {
        public override object OnPost(AdventureType request)
        {
            request.Id = Guid.NewGuid().ToString();
            return request;
        }
        public override object OnGet(AdventureType request)
        {
            if (!string.IsNullOrEmpty(request.Name))
            {
                return request;
            }
            return new List<AdventureType>();
        }
    }
    public class NearByLocationSearch : RestServiceBase<NearByAdventureLocations>
    {
        public override object OnGet(NearByAdventureLocations request)
        {
            if (!string.IsNullOrEmpty(request.LatLon))
            {
                var values = request.SplitLatLon();
                request.Lat = values[0];
                request.Lon = values[1];
            }

            var setting = new ConnectionSettings(Settings.ElasticLocationServer, 9200);
            setting.SetDefaultIndex("pins");
            var client = new ElasticClient(setting);

            string defaultRangeSetting = Settings.DefaultLocationSearchRange;
            request.ValidateRange(defaultRangeSetting);

            var results = client.Search<AdventureLocation>(s => s
                                                            .From(0)  // skip
                                                            .Size(10) // limit
                                                            .Filter(f => f
                                                                        .GeoDistance("geo.location", filter => filter
                                                                                                                .Location(request.Lat, request.Lon)
                                                                                                                .Distance(request.Range)))
                                                            .Index("pins")  // which index
                                                            .Type("region") // what type in the index
                );

            var response = new NearBySearchResponse(request) { Result = results.Documents.ToList() };



            return response;
        }
    }

    public class NearBySearchResponse : IHasResponseStatus
    {
        public NearBySearch Request { get; set; }

        public NearBySearchResponse(NearBySearch request)
        {
            Request = request;
        }

        public List<AdventureLocation> Result { get; set; }

        public ResponseStatus ResponseStatus { get; set; }
    }
}