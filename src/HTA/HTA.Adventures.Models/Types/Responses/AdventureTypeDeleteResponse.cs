using System.Runtime.Serialization;

namespace HTA.Adventures.Models.Types.Responses
{
    [DataContract]
    public class AdventureTypeDeleteResponse : AdventureTypeBaseResponse
    {

        [DataMember]
        public bool Success { get; set; }

        public AdventureTypeDeleteResponse(AdventureType request)
            : base(request)
        {
            Success = false;
        }

        public override bool Equals(object obj)
        {
            AdventureTypeDeleteResponse other = obj as AdventureTypeDeleteResponse;
            if (base.Equals(obj) && null != other)
            {
                return Success == other.Success;
            }
            return false;
        }

    }
}