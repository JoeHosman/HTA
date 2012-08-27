using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Nest;

namespace HTA.Adventures.Models.Types
{
    [DataContract]
    public class LocationPoint
    {
        [ElasticProperty(Name = "lon")]
        [Required]
        [Range(-90.0, 90.0, ErrorMessage = "A valid lon between -90 and 90 must be presented")]
        [DataMember]
        public double Lon { get; set; }

        [ElasticProperty(Name = "lat")]
        [Required]
        [Range(-90.0, 90.0, ErrorMessage = "A valid lat between -90 and 90 must be presented")]
        [DataMember]
        public double Lat { get; set; }

        public LocationPoint()
        {
            Lat = Lon = double.MinValue;
        }

        public string LatLon
        {
            get { return string.Format("{0}, {1}", Lat, Lon); }
        }
    }
}