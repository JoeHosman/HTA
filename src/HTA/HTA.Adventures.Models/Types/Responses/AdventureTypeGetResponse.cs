using System.Collections.Generic;
using System.Runtime.Serialization;
using ServiceStack.ServiceInterface.ServiceModel;

namespace HTA.Adventures.Models.Types.Responses
{
    [DataContract]
    public class AdventureTypeGetResponse : IHasResponseStatus
    {
        [DataMember]
        public AdventureType Request { get; set; }

        [DataMember]
        public IList<AdventureType> AdventureTypes { get; set; }

        public AdventureTypeGetResponse(AdventureType request)
        {
            Request = request;
            AdventureTypes = new List<AdventureType>();
        }

        public override bool Equals(object obj)
        {
            AdventureTypeGetResponse other = obj as AdventureTypeGetResponse;
            if (other != null)
            {
                if (null == Request && null == other.Request)
                    return true;
                if (null != Request)
                {
                    if (!Request.Equals(other.Request))
                        return false;

                    if (null == AdventureTypes && null == other.AdventureTypes)
                        return true;

                    return (null != AdventureTypes && AdventureTypes.Count.Equals(other.AdventureTypes.Count));

                }
            }

            return base.Equals(obj);
        }

        #region Implementation of IHasResponseStatus

        [DataMember]
        public ResponseStatus ResponseStatus { get; set; }

        #endregion
    }
}