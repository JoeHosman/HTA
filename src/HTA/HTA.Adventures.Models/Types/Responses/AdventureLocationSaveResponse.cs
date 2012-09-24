using System.Runtime.Serialization;

namespace HTA.Adventures.Models.Types.Responses
{
    [DataContract]
    public class AdventureLocationSaveResponse : AdventureLocationBaseResponse
    {
        [DataMember]
        public AdventureLocation AdventureLocation { get; set; }

        public AdventureLocationSaveResponse(AdventureLocation request)
            : base(request)
        {
        }

        public override bool Equals(object obj)
        {
            var other = obj as AdventureLocationSaveResponse;
            if (base.Equals(obj) && null != other)
            {
                if ((null != AdventureLocation) != (null != other.AdventureLocation))
                    return false;

                if (null == AdventureLocation)
                    return true;

                return (string.Compare(AdventureLocation.Id, other.AdventureLocation.Id) == 0);
            }
            return false;
        }
    }
}