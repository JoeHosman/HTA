using System.Runtime.Serialization;
using DreamSongs.MongoRepository;
using MongoDB.Bson.Serialization.Attributes;
using Nest;

namespace HTA.Adventures.Models.Types
{
    [DataContract]
    [BsonKnownTypes(typeof(AdventureRegion), typeof(AdventureLocation))]
    public class AdventureSpot : Entity
    {
        [ElasticProperty(Name = "geo_location")]
        [DataMember]
        public LocationPoint LocationPoint { get; set; }

        [ElasticProperty(Name = "name")]
        [DataMember]
        public string Name { get; set; }

        [ElasticProperty(Name = "address")]
        [DataMember]
        public string Address { get; set; }

        [ElasticProperty(Name = "picture")]
        [DataMember]
        public string Picture { get; set; }

        public AdventureSpot()
        {

        }

        public AdventureSpot(LocationPoint locationPoint, string name, string address, string picture)
        {
            LocationPoint = locationPoint;
            Name = name;
            Address = address;
            Picture = picture;
        }
    }
}
