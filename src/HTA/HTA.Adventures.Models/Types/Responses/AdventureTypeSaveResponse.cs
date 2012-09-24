using System.Runtime.Serialization;

namespace HTA.Adventures.Models.Types.Responses
{
    [DataContract]
    public class AdventureTypeSaveResponse : AdventureTypeBaseResponse
    {
        [DataMember]
        public AdventureType AdventureType { get; set; }

        public AdventureTypeSaveResponse(AdventureType request)
            : base(request)
        {
        }

        public override bool Equals(object obj)
        {
            AdventureTypeSaveResponse other = obj as AdventureTypeSaveResponse;
            if (base.Equals(obj) && null != other)
            {
                if (null == AdventureType && null == other.AdventureType)
                    return true;

                if (null != AdventureType)
                    return AdventureType.Equals(other.AdventureType);
            }

            return false;
        }

    }
}