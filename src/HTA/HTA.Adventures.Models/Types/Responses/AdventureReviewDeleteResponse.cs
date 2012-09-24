using System.Runtime.Serialization;

namespace HTA.Adventures.Models.Types.Responses
{
    [DataContract]
    public class AdventureReviewDeleteResponse : AdventureReviewBaseResponse
    {
        [DataMember]
        public bool Success { get; set; }

        public AdventureReviewDeleteResponse(AdventureReview request)
            : base(request)
        {
            Success = false;
        }

        public override bool Equals(object obj)
        {
            var other = obj as AdventureReviewDeleteResponse;
            if (base.Equals(obj) && null != other)
            {

            }

            return false;
        }
    }
}