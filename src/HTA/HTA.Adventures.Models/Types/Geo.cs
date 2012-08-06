using Nest;

namespace HTA.Adventures.Models.Types
{
    public class Geo
    {
        [ElasticProperty(Name = "location")]
        public Location Location { get; set; }
    }
}