using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using DreamSongs.MongoRepository;
using ServiceStack.ServiceHost;

namespace HTA.Adventures.Models.Types
{
    [RestService("/Adventure/Types")]
    [RestService("/Adventure/Types/{Id}")]
    [DataContract]
    public class AdventureType : Entity
    {
        public AdventureType()
        {
            DataCardTemplates = new List<AdventureTypeTemplate>();
        }

        [DataMember]
        [Required]
        [Display(Name = "Type Description")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [DataMember]
        [Display(Name = "Icon Uri Address")]
        [DataType(DataType.ImageUrl)]
        public string IconLink { get; set; }

        [Required]
        [RegularExpression(@"(\S)+", ErrorMessage = "White space is not allowed")]
        [StringLength(30, MinimumLength = 3)]
        [Display(Name = "Type Name")]
        [DataMember]
        public string Name { get; set; }


        [DataMember]
        [Display(Name = "Picture Uri Address")]
        [DataType(DataType.ImageUrl)]
        public string PhotoLink { get; set; }

        [DataMember]
        public List<AdventureTypeTemplate> DataCardTemplates { get; set; }
    }

    [RestService("/Adventure/Type/DataCards/{Id}")]
    public class AdventureTypeDataCards
    {
        public string Id { get; set; }
    }

    public class AdventureTypeDataCardsResponse
    {
        public string Id { get; set; }
        public IList<AdventureDataCard> DataCards { get; set; }
    }
}