using System.Runtime.Serialization;
using DreamSongs.MongoRepository;
using MongoDB.Bson.Serialization.Attributes;
using Nest;

namespace HTA.Adventures.Models.Types
{
    /// <summary>
    /// Represents a spot somewhere in the world with a Geo location point and a name.
    /// </summary>
    [DataContract]
    [BsonKnownTypes(typeof(Region), typeof(AdventureLocation))]
    public class Spot : Entity
    {
        /// <summary>
        /// Get location Point
        /// </summary>
        [ElasticProperty(Name = "geo_location")]
        [DataMember]
        public GeoPoint Point { get; set; }

        /// <summary>
        /// Spot's name
        /// </summary>
        [ElasticProperty(Name = "name")]
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// Spot's Address
        /// </summary>
        [ElasticProperty(Name = "address")]
        [DataMember]
        public string Address { get; set; }

        /// <summary>
        /// Spot's Picture
        /// </summary>
        [ElasticProperty(Name = "picture")]
        [DataMember]
        public string Picture { get; set; }

        public Spot()
        {
            Name = "";
            Point = new GeoPoint();
        }

        public Spot(GeoPoint geoPoint, string name)
        {
            Point = geoPoint;
            Name = name;
        }
    }
}
