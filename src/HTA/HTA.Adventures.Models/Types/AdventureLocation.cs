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
        public AdventureLocation(LocationPoint locationPoint, string name, string address, string picture)
            : base(locationPoint, name, address, picture)
        {
            AdventureRegion = new AdventureRegion(locationPoint, name, address, picture);
        }

        [ElasticProperty(Name = "region")]
        [DataMember]
        public AdventureRegion AdventureRegion { get; set; }

    }
}