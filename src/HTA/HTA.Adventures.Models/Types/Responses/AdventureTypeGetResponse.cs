using System.Collections.Generic;
using System.Runtime.Serialization;

namespace HTA.Adventures.Models.Types.Responses
{
    [DataContract]
    public class AdventureTypeGetResponse : AdventureTypeBaseResponse
    {
        [DataMember]
        public IList<AdventureType> AdventureTypes { get; set; }

        public AdventureTypeGetResponse(AdventureType request)
            : base(request)
        {
            AdventureTypes = new List<AdventureType>();
        }

        public override bool Equals(object obj)
        {
            AdventureTypeGetResponse other = obj as AdventureTypeGetResponse;
            if (base.Equals(obj) && null != other)
            {
                if (null == AdventureTypes && null == other.AdventureTypes)
                    return true;

                return (null != AdventureTypes && AdventureTypes.Count.Equals(other.AdventureTypes.Count));
            }

            return false;
        }

        #region Implementation of IHasResponseStatus

        #endregion
    }
}