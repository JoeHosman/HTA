using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using DreamSongs.MongoRepository;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface.ServiceModel;

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
        public string Name { get; set; }

        
        [DataMember]
        [Required]
        public AdventureType AdventureType { get; set; }

        [DataMember]
        [Required]
        public IList<AdventureDataCard> DataCards { get; set; }

        [DataMember]
        [Required]
        public AdventureLocation AdventureLocation { get; set; }
    }

    public class AdventureReviewResponse : IHasResponseStatus
    {
        public AdventureReviewResponse(AdventureReview request)
        {
            Request = request;
        }

        public AdventureReview Request { get; set; }

        public AdventureReview Review { get; set; }

        #region Implementation of IHasResponseStatus

        public ResponseStatus ResponseStatus { get; set; }

        #endregion
    }
}
