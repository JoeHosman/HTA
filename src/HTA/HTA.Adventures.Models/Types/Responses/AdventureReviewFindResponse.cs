using System.Collections.Generic;
using System.Runtime.Serialization;

namespace HTA.Adventures.Models.Types.Responses
{
    [DataContract]
    public class AdventureReviewFindResponse : AdventureReviewBaseResponse
    {
        [DataMember]
        public IList<AdventureReview> AdventureReviews { get; set; }

        public AdventureReviewFindResponse(AdventureReview request)
            : base(request)
        {
            AdventureReviews = new List<AdventureReview>();
        }

        public override bool Equals(object obj)
        {
            var other = obj as AdventureReviewFindResponse;
            if (base.Equals(obj) && null != other)
            {

            }
            return false;
        }
    }
}