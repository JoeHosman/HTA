using System.Collections.Generic;
using System.Linq;
using HTA.Adventures.Models;
using HTA.Adventures.Models.Types;
using Nest;

namespace HTA.Adventures.API.WebService.Data
{
    internal class ElasticAdventureLocationSearchRepository : IAdventureLocationSearchRepository
    {
        private readonly string _elasticServer;

        public ElasticAdventureLocationSearchRepository(string elasticLocationServer)
        {
            _elasticServer = elasticLocationServer;
        }

        public List<Spot> GetNearByAdventureLocations(GeoPoint point, GeoRange range)
        {
            var setting = new ConnectionSettings(_elasticServer, 9200);
            setting.SetDefaultIndex("pins");

            //request.ValidateRange(_defaultRangeSetting);


            var client = new ElasticClient(setting);

            var results = client.Search<Spot>(s => s
                .From(0) // skip
                .Size(10) // limit
                .Filter(f => f
                        .GeoDistance("geo.location", filter =>
                            filter
                            .Location(point.Lat, point.Lon)
                            .Distance(range.ToString())))
                .Index("pins") // which index
                .Type("region") // what type in the index
                );


            return results.Documents.ToList();
        }
    }
}