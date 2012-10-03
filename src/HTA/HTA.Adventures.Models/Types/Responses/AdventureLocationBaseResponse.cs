using System.Runtime.Serialization;
using ServiceStack.ServiceInterface.ServiceModel;

namespace HTA.Adventures.Models.Types.Responses
{
    [DataContract]
    public class AdventureLocationBaseResponse : IHasResponseStatus
    {
        public AdventureLocationBaseResponse(AdventureLocation request)
        {
            Request = request;
        }

        [DataMember]
        public AdventureLocation Request { get; set; }

        [DataMember]
        public ResponseStatus ResponseStatus { get; set; }

        public override bool Equals(object obj)
        {
            var other = obj as AdventureLocationBaseResponse;

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
    }
}