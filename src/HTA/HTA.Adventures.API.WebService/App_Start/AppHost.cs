using HTA.Adventures.API.ServiceInterface;
using HTA.Adventures.API.WebService.Data;
using HTA.Adventures.Models;
using ServiceStack.WebHost.Endpoints;

[assembly: WebActivator.PreApplicationStartMethod(typeof(HTA.Adventures.API.WebService.App_Start.AppHost), "Start")]


/**
 * Entire ServiceStack Starter Template configured with a 'Hello' Web Service and a 'Todo' Rest Service.
 *
 * Auto-Generated Metadata API page at: /metadata
 * See other complete web service examples at: https://github.com/ServiceStack/ServiceStack.Examples
 */

namespace HTA.Adventures.API.WebService.App_Start
{
    public class AppHost
        : AppHostBase
    {
        public AppHost() //Tell ServiceStack the name and where to find your web services
            : base("StarterTemplate ASP.NET Host", typeof(AdventureReviewService).Assembly) { }

        public override void Configure(Funq.Container container)
        {
            //Set JSON web services to return idiomatic JSON camelCase properties
            ServiceStack.Text.JsConfig.EmitCamelCaseNames = true;

            //Configure User Defined REST Paths
            //Routes
            //    .Add<Hello>("/hello")
            //    .Add<Hello>("/hello/{Name*}")
            //    .Add<Todo>("/todos")
            //    .Add<Todo>("/todos/{Id}");
            //.Add<NearByAdventureLocations>("/adventure/locations/{LatLon}");



            //Uncomment to change the default ServiceStack configuration
            //SetConfig(new EndpointHostConfig {
            //    DebugMode = true, //Show StackTraces when developing
            //});

            //Enable Authentication
            //ConfigureAuth(container);

            //Register all your dependencies
            //container.Register(new TodoRepository());

            // Add our mongo adventure type repo into the system
            container.Register(new MongoAdventureTypeRepository());

            var adventureLocationSearchRepository = new ElasticAdventureLocationSearchRepository
                                                        {ElasticServer = Settings.ElasticLocationServer};

            container.Register(adventureLocationSearchRepository);

            // Register our mongo adapter as the IAdventuretypeRepository to use :D
            container.RegisterAs<MongoAdventureTypeRepository, IAdventureReviewRepository>();
            container.RegisterAs<MongoAdventureTypeRepository, IAdventureTypeRepository>();
            container.RegisterAs<MongoAdventureTypeRepository, IAdventureTypeTemplateRepository>();
            container.RegisterAs<MongoAdventureTypeRepository, IAdventureRegionRepository>();
            container.RegisterAs<MongoAdventureTypeRepository, IAdventureLocationRepository>();

            container.RegisterAs<ElasticAdventureLocationSearchRepository, IAdventureLocationSearchRepository>();
        }

        /* Uncomment to enable ServiceStack Authentication and CustomUserSession
        private void ConfigureAuth(Funq.Container container)
        {
            var appSettings = new AppSettings();

            //Default route: /auth/{provider}
            Plugins.Add(new AuthFeature(this, () => new CustomUserSession(),
                new IAuthProvider[] {
                    new CredentialsAuthProvider(appSettings), 
                    new FacebookAuthProvider(appSettings), 
                    new TwitterAuthProvider(appSettings), 
                    new BasicAuthProvider(appSettings), 
                })); 

            //Default route: /register
            Plugins.Add(new RegistrationFeature()); 

            //Requires ConnectionString configured in Web.Config
            var connectionString = ConfigurationManager.ConnectionStrings["AppDb"].ConnectionString;
            container.Register<IDbConnectionFactory>(c =>
                new OrmLiteConnectionFactory(connectionString, SqlServerOrmLiteDialectProvider.Instance));

            container.Register<IUserAuthRepository>(c =>
                new OrmLiteAuthRepository(c.Resolve<IDbConnectionFactory>()));

            var authRepo = (OrmLiteAuthRepository)container.Resolve<IUserAuthRepository>();
            authRepo.CreateMissingTables();
        }
        */

        public static void Start()
        {
            new AppHost().Init();
        }
    }
}
