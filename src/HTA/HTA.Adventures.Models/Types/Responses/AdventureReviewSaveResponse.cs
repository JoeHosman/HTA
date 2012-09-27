using System.Runtime.Serialization;

namespace HTA.Adventures.Models.Types.Responses
{
    [DataContract]
    public class AdventureReviewSaveResponse : AdventureReviewBaseResponse
    {
        [DataMember]
        public AdventureReview AdventureReview { get; set; }

        public AdventureReviewSaveResponse(AdventureReview request)
            : base(request)
        {
        }

        public override bool Equals(object obj)
        {
            var other = obj as AdventureReviewSaveResponse;
            if (base.Equals(obj) && null != other)
            {
                if ((null != AdventureReview) != (null != other.AdventureReview))
                    return false;

                if (null == AdventureReview)
                    return true;

                if (string.IsNullOrEmpty(AdventureReview.Id) != string.IsNullOrEmpty(other.AdventureReview.Id))
                    return false;

                if (string.IsNullOrEmpty(AdventureReview.Id))
                    return true;

                return (string.Compare(AdventureReview.Id, other.AdventureReview.Id) == 0);
            }
            return false;
        }
    }
}