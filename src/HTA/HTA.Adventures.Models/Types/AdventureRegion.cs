using System.Runtime.Serialization;
using DreamSongs.MongoRepository;
using Nest;
using ServiceStack.ServiceHost;

namespace HTA.Adventures.Models.Types
{
    [ElasticType(Name = "region", IdProperty = "id")]
    [DataContract]
    [RestService("/Adventure/Regions")]
    [RestService("/Adventure/Regions/{Id}")]
    [CollectionName("AdventureRegions")]
    public class AdventureRegion : AdventureSpot
    {
        public AdventureRegion()
            : base()
        {

        }
        public AdventureRegion(LocationPoint locationPoint, string name, string address, string picture)
            : base(locationPoint, name, address, picture)
        {
        }
    }
}