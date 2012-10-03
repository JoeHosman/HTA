using System.Collections.Generic;
using System.Runtime.Serialization;

namespace HTA.Adventures.Models.Types.Responses
{
    [DataContract]
    public class AdventureReviewGetResponse : AdventureReviewBaseResponse
    {
        [DataMember]
        public IList<AdventureReview> AdventureReviews { get; set; }

        public AdventureReviewGetResponse(AdventureReview request)
            : base(request)
        {
            AdventureReviews = new List<AdventureReview>();
        }
        public override bool Equals(object obj)
        {
            var other = obj as AdventureReviewGetResponse;
            if (base.Equals(obj) && null != other)
            {
                if ((null != AdventureReviews) != (null != other.AdventureReviews))
                    return false;
                if (null == AdventureReviews)
                    return true;

                return (AdventureReviews.Count == other.AdventureReviews.Count);

            }
            return false;
        }
    }
}