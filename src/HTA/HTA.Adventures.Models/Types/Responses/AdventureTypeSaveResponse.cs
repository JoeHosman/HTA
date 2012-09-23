using System.Runtime.Serialization;
using ServiceStack.ServiceInterface.ServiceModel;

namespace HTA.Adventures.Models.Types.Responses
{
    [DataContract]
    public class AdventureTypeSaveResponse : IHasResponseStatus
    {
        [DataMember]
        public AdventureType Request { get; set; }

        [DataMember]
        public AdventureType AdventureType { get; set; }

        public AdventureTypeSaveResponse(AdventureType request)
        {
            Request = request;
        }

        public override bool Equals(object obj)
        {
            AdventureTypeSaveResponse other = obj as AdventureTypeSaveResponse;
            if (other != null)
            {
                if (null == Request && null == other.Request)
                    return true;
                if (null != Request)
                {
                    if (!Request.Equals(other.Request))
                        return false;

                    if (null == AdventureType && null == other.AdventureType)
                        return true;

                    if (null != AdventureType)
                        return AdventureType.Equals(other.AdventureType);
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