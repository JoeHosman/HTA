using System.Collections.Generic;
using System.Linq;
using HTA.Adventures.Models;
using HTA.Adventures.Models.Types;
using Nest;

namespace HTA.Adventures.API.WebService.Data
{
    internal class ElasticAdventureLocationSearchRepository : IAdventureLocationSearchRepository
    {
        public static string ElasticServer { get; set; }

        public List<AdventureLocation> GetNearByAdventureLocations(GeoPoint point, GeoRange range)
        {

            var locations = new List<AdventureLocation>();

            var setting = new ConnectionSettings(ElasticServer, 9200);
            setting.SetDefaultIndex("adventure");


            var client = new ElasticClient(setting);

            var results = client.Search<AdventureLocation>(s => s
                .From(0) // skip
                .Size(10) // limit
                .Filter(f => f
                        .GeoDistance("geo_location", filter =>
                            filter
                            .Location(point.Lat, point.Lon)
                            .Distance(range.ToString())))
                .Index("adventure") // which index
                .Type("location") // what type in the index
                );


            return results.Documents.ToList();

        }
    }
}