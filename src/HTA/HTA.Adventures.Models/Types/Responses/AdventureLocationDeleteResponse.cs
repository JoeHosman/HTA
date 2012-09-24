using System.Runtime.Serialization;
using HTA.Adventures.Models.Types.Responses;

namespace HTA.Adventures.Models.Types
{
    [DataContract]
    public class AdventureLocationDeleteResponse : AdventureLocationBaseResponse
    {
        public AdventureLocationDeleteResponse(AdventureLocation request)
            : base(request)
        {
        }

        public override bool Equals(object obj)
        {
            var other = obj as AdventureLocationDeleteResponse;
            if (base.Equals(obj) && null != other)
            {

            }
            return false;
        }
    }
}