using System.Collections.Generic;
using System.Runtime.Serialization;
using ServiceStack.ServiceInterface.ServiceModel;

namespace HTA.Adventures.Models.Types.Responses
{
    [DataContract]
    public class AdventureTypeFindResponse : IHasResponseStatus
    {
        [DataMember]
        public AdventureType Request { get; set; }

        [DataMember]
        public IList<AdventureType> AdventureTypeResults { get; set; }

        public AdventureTypeFindResponse(AdventureType request)
        {
            Request = request;
        }

        #region Implementation of IHasResponseStatus

        [DataMember]
        public ResponseStatus ResponseStatus { get; set; }

        #endregion
    }
}