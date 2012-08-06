using System.Runtime.Serialization;
using DreamSongs.MongoRepository;
using Nest;

namespace HTA.Adventures.Models.Types
{
    [ElasticType(Name = "adventure_type_template_ref", IdProperty = "id")]
    [DataContract]
    public class AdventureTypeTemplateReference : Entity
    {
        [ElasticProperty(Name = "name")]
        [DataMember]
        public string Name { get; set; }
    }
}