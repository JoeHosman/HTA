using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using DreamSongs.MongoRepository;

namespace HTA.Adventures.Models.Types
{
    [DataContract]
    public class AdventureReview : Entity
    {
        [Required]
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public AdventureType AdventureType { get; set; }
    }
}
