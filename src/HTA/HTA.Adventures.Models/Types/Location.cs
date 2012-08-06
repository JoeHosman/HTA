using Nest;

namespace HTA.Adventures.Models.Types
{
    public class Location
    {
        [ElasticProperty(Name = "lon")]
        public double Lon { get; set; }
        [ElasticProperty(Name = "lat")]
        public double Lat { get; set; }

    }
}