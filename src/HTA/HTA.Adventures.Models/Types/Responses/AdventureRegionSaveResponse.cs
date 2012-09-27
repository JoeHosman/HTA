using System.Runtime.Serialization;

namespace HTA.Adventures.Models.Types.Responses
{
    [DataContract]
    public class AdventureRegionSaveResponse : AdventureRegionBaseResponse
    {
        [DataMember]
        public Region AdventureRegion { get; set; }

        public AdventureRegionSaveResponse(Region request)
            : base(request)
        {

        }

        public override bool Equals(object obj)
        {
            var other = obj as AdventureRegionSaveResponse;
            if (base.Equals(obj) && null != other)
            {
                if ((null != AdventureRegion) != (null != other.AdventureRegion))
                    return false;
                if (null == AdventureRegion)
                    return true;

                return (string.Compare(AdventureRegion.Id, other.AdventureRegion.Id) == 0);
            }
            return false;
        }
    }
}