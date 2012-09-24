using System.Runtime.Serialization;
using ServiceStack.ServiceInterface.ServiceModel;

namespace HTA.Adventures.Models.Types.Responses
{
    [DataContract]
    public class AdventureReviewBaseResponse : IHasResponseStatus
    {
        public AdventureReviewBaseResponse(AdventureReview request)
        {
            Request = request;
        }
        [DataMember]
        public AdventureReview Request { get; set; }

        public override bool Equals(object obj)
        {
            var other = obj as AdventureReviewBaseResponse;
            if (null != other)
            {
                if ((null != Request) != (null != other.Request))
                    return false;

                if (null == Request)
                    return true;

                return (null != Request && string.Compare(Request.Id, other.Request.Id) == 0);

            }

            return false;
        }

        #region Implementation of IHasResponseStatus

        [DataMember]
        public ResponseStatus ResponseStatus { get; set; }

        #endregion
    }
}