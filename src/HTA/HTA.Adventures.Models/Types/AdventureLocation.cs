using System.Runtime.Serialization;
using DreamSongs.MongoRepository;
using Nest;
using ServiceStack.ServiceHost;

namespace HTA.Adventures.Models.Types
{
    [ElasticType(Name = "location", IdProperty = "id")]
    [DataContract]
    [RestService("/Adventure/Locations")]
    [RestService("/Adventure/Locations/{Id}")]
    [CollectionName("AdventureLocations")]
    public class AdventureLocation : Spot
    {
        public AdventureLocation()
        { Region = new Region(); }

        public AdventureLocation(GeoPoint geoPoint, string name)
            : base(geoPoint, name)
        {
            Region = new Region(geoPoint, name);
        }

        public AdventureLocation(Region region)
        {
            Point = region.Point;
            Address = region.Address;
            Name = region.Name;
            Picture = region.Picture;
            Region = region;
        }

        [ElasticProperty(Name = "region")]
        [DataMember]
        public Region Region { get; set; }

    }
}