using System.Runtime.Serialization;
using ServiceStack.ServiceInterface.ServiceModel;

namespace HTA.Adventures.Models.Types.Responses
{
    [DataContract]
    public class AdventureTypeBaseResponse : IHasResponseStatus
    {
        [DataMember]
        public AdventureType Request { get; set; }

        [DataMember]
        public ResponseStatus ResponseStatus { get; set; }

        public AdventureTypeBaseResponse(AdventureType request)
        {
            Request = request;
        }

        public override bool Equals(object obj)
        {
            var other = obj as AdventureTypeBaseResponse;
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