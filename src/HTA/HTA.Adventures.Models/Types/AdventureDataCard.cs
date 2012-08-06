using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using DreamSongs.MongoRepository;
using Nest;

namespace HTA.Adventures.Models.Types
{
    [ElasticType(Name = "adventure_data_card", IdProperty = "id")]
    [DataContract]
    public class AdventureDataCard : Entity
    {
        [ElasticProperty(Name = "title")]
        [DataMember]
        public string Title { get; set; }

        [ElasticProperty(Name = "body")]
        [DataMember]
        [DataType(DataType.MultilineText)]
        public string Body { get; set; }
    }
}