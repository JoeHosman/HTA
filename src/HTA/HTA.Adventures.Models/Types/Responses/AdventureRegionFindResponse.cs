using System.Collections.Generic;
using System.Runtime.Serialization;

namespace HTA.Adventures.Models.Types.Responses
{
    [DataContract]
    public class AdventureRegionFindResponse : AdventureRegionBaseResponse
    {
        [DataMember]
        public IList<Region> AdventureRegions { get; set; }

        public AdventureRegionFindResponse(Region request)
            : base(request)
        {
            AdventureRegions = new List<Region>();
        }

        public override bool Equals(object obj)
        {
            var other = obj as AdventureRegionFindResponse;
            if (base.Equals(obj) && null != other)
            {
                if ((null != AdventureRegions) != (null != other.AdventureRegions))
                    return false;
                if (null == AdventureRegions)
                    return true;

                return (AdventureRegions.Count == other.AdventureRegions.Count);
            }

            return false;
        }
    }
}