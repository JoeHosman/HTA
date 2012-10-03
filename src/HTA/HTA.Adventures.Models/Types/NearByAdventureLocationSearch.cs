using System.Runtime.Serialization;
using ServiceStack.ServiceHost;

namespace HTA.Adventures.Models.Types
{
    [DataContract]
    [RestService("/Adventure/Locations/NearBy/{LatLon}")]
    public class NearByAdventureLocationSearch
    {
        [DataMember]
        public GeoPoint Point { get; set; }

        [DataMember]
        public string LatLon
        {
            get { return Point.LatLon; }
            set { Point.LatLon = value; }
        }

        public GeoRange Range { get; set; }

        public NearByAdventureLocationSearch()
        {
            Point = new GeoPoint();
            Range = new GeoRange();
        }
    }
}