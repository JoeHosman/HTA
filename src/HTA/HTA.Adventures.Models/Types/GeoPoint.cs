using System.Runtime.Serialization;
using Nest;

namespace HTA.Adventures.Models.Types
{
    /// <summary>
    /// Represents a get location point with  Lat and Lon values.
    /// </summary>
    [DataContract]
    public class GeoPoint
    {
        /// <summary>
        /// Longitude 
        /// </summary>
        [ElasticProperty(Name = "lon")]
        [DataMember]
        public double Lon { get; set; }

        /// <summary>
        /// Latitude
        /// </summary>
        [ElasticProperty(Name = "lat")]
        [DataMember]
        public double Lat { get; set; }

        public GeoPoint()
        {
            LatLon = string.Empty;
        }

        public string LatLon
        {
            get { return string.Format("{0}, {1}", Lat, Lon); }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    Lat = double.MinValue;
                    Lon = double.MinValue;
                }
                else
                {
                    var values = value.Split(new[] { ',' }, 2);
                    Lat = double.Parse(values[0]);
                    Lon = double.Parse(values[1]);
                }
            }
        }
    }
}