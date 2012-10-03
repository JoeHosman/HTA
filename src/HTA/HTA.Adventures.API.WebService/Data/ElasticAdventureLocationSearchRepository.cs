using System.Collections.Generic;
using System.Linq;
using HTA.Adventures.Models;
using HTA.Adventures.Models.Types;
using Nest;

namespace HTA.Adventures.API.WebService.Data
{
    internal class ElasticAdventureLocationSearchRepository : IAdventureLocationSearchRepository
    {
        public string ElasticServer { get; set; }

        public List<AdventureLocation> GetNearByAdventureLocations(GeoPoint point, GeoRange range)
        {

            var locations = new List<AdventureLocation>
                                {
                                    new AdventureLocation(point, "test00"),
                                    new AdventureLocation(point, "test01"),
                                    new AdventureLocation(point, "test02"),
                                    new AdventureLocation(point, "test03"),
                                    new AdventureLocation(point, "test04"),
                                    new AdventureLocation(point, "test05"),
                                    new AdventureLocation(point, "test06"),
                                    new AdventureLocation(point, "test07"),
                                    new AdventureLocation(point, "test08")
                                };

            return locations;

/*
            var setting = new ConnectionSettings(ElasticServer, 9200);
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
                .Type("location") // what type in the index
                );


            return results.Documents.ToList();
*/
        }
    }
}