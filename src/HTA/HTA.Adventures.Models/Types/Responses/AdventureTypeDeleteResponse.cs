using System.Runtime.Serialization;
using ServiceStack.ServiceInterface.ServiceModel;

namespace HTA.Adventures.Models.Types.Responses
{
    [DataContract]
    public class AdventureTypeDeleteResponse : IHasResponseStatus
    {
        [DataMember]
        public AdventureType Request { get; set; }

        [DataMember]
        public bool Success { get; set; }

        public AdventureTypeDeleteResponse(AdventureType request)
        {
            Request = request;
            Success = false;
        }

        public override bool Equals(object obj)
        {
            AdventureTypeDeleteResponse other = obj as AdventureTypeDeleteResponse;
            if (null != other)
            {
                return Success == other.Success;
            }
            return base.Equals(obj);
        }

        #region Implementation of IHasResponseStatus

        [DataMember]
        public ResponseStatus ResponseStatus { get; set; }

        #endregion
    }
}