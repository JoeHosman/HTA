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
    public class AdventureLocation : AdventureSpot
    {
        public AdventureLocation() : base() { AdventureRegion = new AdventureRegion(); }
        public AdventureLocation(LocationPoint locationPoint, string name)
            : base(locationPoint, name)
        {
        }

        public AdventureLocation(AdventureRegion adventureRegion)
        {
            base.LocationPoint = adventureRegion.LocationPoint;
            base.Address = adventureRegion.Address;
            base.Name = adventureRegion.Name;
            base.Picture = adventureRegion.Picture;
            AdventureRegion = adventureRegion;
        }

        [ElasticProperty(Name = "region")]
        [DataMember]
        public AdventureRegion AdventureRegion { get; set; }

    }
}