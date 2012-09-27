using System.Collections.Generic;
using System.Runtime.Serialization;

namespace HTA.Adventures.Models.Types.Responses
{
    [DataContract]
    public class AdventureLocationFindResponse : AdventureLocationBaseResponse
    {
        [DataMember]
        public IList<AdventureLocation> AdventureLocations { get; set; }

        public AdventureLocationFindResponse(AdventureLocation request)
            : base(request)
        {
            AdventureLocations = new List<AdventureLocation>();
        }

        public override bool Equals(object obj)
        {
            var other = obj as AdventureLocationFindResponse;
            if (base.Equals(obj) && null != other)
            {
                if ((null != AdventureLocations) != (null != other.AdventureLocations))
                    return false;

                if (null == AdventureLocations)
                    return true;

                return (AdventureLocations.Count == other.AdventureLocations.Count);

            }
            return false;
        }
    }
}