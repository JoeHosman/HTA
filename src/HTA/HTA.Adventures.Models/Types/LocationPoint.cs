using Nest;

namespace HTA.Adventures.Models.Types
{
    public class LocationPoint
    {
        [ElasticProperty(Name = "lon")]
        public double Lon { get; set; }
        [ElasticProperty(Name = "lat")]
        public double Lat { get; set; }


        public string LatLon
        {
            get { return string.Format("{0}, {1}", Lat, Lon); }
        }
    }
}