using System.Collections.Generic;
using System.Linq;
using DreamSongs.MongoRepository;
using HTA.Adventures.Models.Types;
using Nest;

namespace HTA.Adventures.Models
{
    public class AdventureLocationRepository : IAdventureLocationRepository
    {
        public AdventureLocationRepository()
        {
            var setting = new ConnectionSettings("office.mtctickets.com", 9200);
            setting.SetDefaultIndex("pins");
            client = new ElasticClient(setting);
        }

        static readonly MongoRepository<AdventureLocation> MongoAdventureLocationRepository = new MongoRepository<AdventureLocation>();
        private readonly ElasticClient client;

        public void Add(AdventureLocation location)
        {
            MongoAdventureLocationRepository.Add(location);

            var result = client.Index(location, "pins", "region", location.Id);

            //it was extra testing
            //string data = JsonConvert.SerializeObject(location);
        }

        public List<AdventureLocation> GetNearBy(Geo geoLocation, GeoRange geoRange)
        {

            var results = client.Search<AdventureLocation>(s => s
                                                                    .From(0)
                                                                    .Size(10)
                                                                    .Filter(f => f
                                                                                     .GeoDistance("geo.location", filter => filter
                                                                                                                                .Location(geoLocation.Location.Lon, geoLocation.Location.Lat)
                                                                                                                                .Distance(string.Format("{0}mi", geoRange.Range))))
                                                                    .Index("pins")
                                                                    .Type("region")
                );
            return results.Documents.ToList();
        }
    }
}