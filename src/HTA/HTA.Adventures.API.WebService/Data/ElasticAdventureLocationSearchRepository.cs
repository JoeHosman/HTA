using System.Collections.Generic;
using System.Linq;
using HTA.Adventures.API.ServiceInterface;
using HTA.Adventures.Models;
using HTA.Adventures.Models.Types;
using Nest;

namespace HTA.Adventures.API.WebService.Data
{
    internal class ElasticAdventureLocationSearchRepository : IAdventureLocationSearchRepository
    {
        private readonly string _elasticServer;
        private readonly string _defaultRangeSetting;

        public ElasticAdventureLocationSearchRepository(string elasticLocationServer, string defaultLocationSearchRange)
        {
            _elasticServer = elasticLocationServer;
            _defaultRangeSetting = defaultLocationSearchRange;
        }

        public List<Spot> GetNearByAdventureLocations(NearByAdventureLocations request)
        {
            var setting = new ConnectionSettings(_elasticServer, 9200);
            setting.SetDefaultIndex("pins");

            request.ValidateRange(_defaultRangeSetting);


            var client = new ElasticClient(setting);

            var results = client.Search<Spot>(s => s
                .From(0) // skip
                .Size(10) // limit
                .Filter(f => f
                        .GeoDistance("geo.location", filter =>
                            filter
                            .Location(request.Lat, request.Lon)
                            .Distance(request.Range)))
                .Index("pins") // which index
                .Type("region") // what type in the index
                );


            return results.Documents.ToList();
        }
    }
}