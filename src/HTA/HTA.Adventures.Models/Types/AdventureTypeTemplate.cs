using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using DreamSongs.MongoRepository;
using ServiceStack.ServiceHost;

namespace HTA.Adventures.Models.Types
{
    [RestService("/Adventure/TypeTemplates")]
    [RestService("/Adventure/TypeTemplates/{Id}")]
    [DataContract]
    public class AdventureTypeTemplate : Entity
    {
        [DataMember]
        [Required]
        [Display(Name = "Type Description")]
        public string Description { get; set; }

        [Required]
        //[RegularExpression(@"(\S)+", ErrorMessage = "White space is not allowed")]
        [StringLength(50, MinimumLength = 3)]
        [Display(Name = "Type Name")]
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public List<AdventureDataCard> DataCards { get; set; }

        public AdventureTypeTemplate()
        {
            DataCards = new List<AdventureDataCard>();
        }

        public override string ToString()
        {
            return Id;
        }
    }
}