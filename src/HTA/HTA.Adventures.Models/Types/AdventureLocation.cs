using System.Runtime.Serialization;
using DreamSongs.MongoRepository;
using Nest;

namespace HTA.Adventures.Models.Types
{
    [ElasticType(Name = "region", IdProperty = "id")]
    [DataContract]
    public class AdventureLocation : Entity
    {
        [ElasticProperty(Name = "geo")]
        [DataMember]
        public Geo Geo { get; set; }

        [ElasticProperty(Name = "name")]
        [DataMember]
        public string Name { get; set; }

        [ElasticProperty(Name = "address")]
        [DataMember]
        public string Address { get; set; }

        [ElasticProperty(Name = "picture")]
        [DataMember]
        public string Picture { get; set; }


        public AdventureLocation(Geo location, string name, string address, string picture)
        {
            Geo = location;
            Name = name;
            Address = address;
            Picture = picture;
        }
    }
}
