using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using DreamSongs.MongoRepository;
using ServiceStack.ServiceHost;

namespace HTA.Adventures.Models.Types
{
    [DataContract]
    [RestService("/Adventure/Reviews")]
    [RestService("/Adventure/Reviews/{Id}")]
    public class AdventureReview : Entity
    {
        public AdventureReview()
        {
            DataCards = new List<AdventureDataCard>();

        }

        [DataMember]
        [Required]
        public string AdventureName { get; set; }

        [DataMember]
        [Required]
        public AdventureType AdventureType { get; set; }

        [DataMember]
        [Required]
        public IList<AdventureDataCard> DataCards { get; set; }

        [DataMember]
        [Required]
        public AdventureLocation AdventureLocation { get; set; }

        [DataMember]
        [Required]
        public TimeSpan? AdventureDuration { get; set; }

        [DataMember]
        [Required]
        public DateTime? AdventureDate { get; set; }
    }
}
