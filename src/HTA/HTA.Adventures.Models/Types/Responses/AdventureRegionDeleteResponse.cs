using System.Runtime.Serialization;

namespace HTA.Adventures.Models.Types.Responses
{
    [DataContract]
    public class AdventureRegionDeleteResponse : AdventureRegionBaseResponse
    {
        [DataMember]
        public bool Success { get; set; }

        public AdventureRegionDeleteResponse(Region request)
            : base(request)
        {
            Success = false;
        }

        public override bool Equals(object obj)
        {
            var other = obj as AdventureRegionDeleteResponse;
            if (base.Equals(obj) && null != other)
            {
                return (Success == other.Success);
            }
            return false;
        }
    }
}