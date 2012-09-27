using System.Runtime.Serialization;
using ServiceStack.ServiceInterface.ServiceModel;

namespace HTA.Adventures.Models.Types.Responses
{
    [DataContract]
    public class AdventureDataCardBaseResponse : IHasResponseStatus
    {
        public AdventureDataCardBaseResponse(AdventureDataCard request)
        {
            Request = request;
        }

        [DataMember]
        public AdventureDataCard Request { get; set; }

        [DataMember]
        public ResponseStatus ResponseStatus { get; set; }

        public override bool Equals(object obj)
        {
            var other = obj as AdventureDataCardBaseResponse;

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