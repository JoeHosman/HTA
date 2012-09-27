using System.Runtime.Serialization;
using ServiceStack.ServiceInterface.ServiceModel;

namespace HTA.Adventures.Models.Types.Responses
{
    [DataContract]
    public class AdventureRegionBaseResponse : IHasResponseStatus
    {
        [DataMember]
        public Region Request { get; set; }

        public AdventureRegionBaseResponse(Region request)
        {
            Request = request;
        }

        public override bool Equals(object obj)
        {
            var other = obj as AdventureRegionBaseResponse;
            if (null != other)
            {
                if ((null != Request) != (null != other.Request))
                    return false;
                if (null == Request)
                    return true;

                return (string.Compare(Request.Id, other.Request.Id) == 0);

            }

            return false;
        }

        #region Implementation of IHasResponseStatus

        [DataMember]
        public ResponseStatus ResponseStatus { get; set; }

        #endregion
    }
}