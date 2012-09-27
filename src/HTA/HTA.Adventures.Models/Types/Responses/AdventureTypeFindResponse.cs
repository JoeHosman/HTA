using System.Collections.Generic;
using System.Runtime.Serialization;

namespace HTA.Adventures.Models.Types.Responses
{
    [DataContract]
    public class AdventureTypeFindResponse : AdventureTypeBaseResponse
    {
        [DataMember]
        public IList<AdventureType> AdventureTypeResults { get; set; }

        public AdventureTypeFindResponse(AdventureType request)
            : base(request)
        {
        }

        public override bool Equals(object obj)
        {
            var other = obj as AdventureTypeFindResponse;
            if (base.Equals(obj) && null != other)
            {

            }
            return false;
        }
    }
}