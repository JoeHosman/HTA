using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HTA.Adventures.Models.Types
{
    public class AdventureRegionInfomationBrief
    {
        public ulong ID { get; set; }

        public string Name { get; set; }
        public string NearBy { get; set; }

        public ulong LocationCount { get; set; }
        public ulong ReviewCount { get; set; }

        public List<AssociatedAdventureType> AssociatedAdventureTypes { get; set; }

        public List<AssociatedAdventureLocation> AssociatedAdventureLocations { get; set; }
    }

    public class AssociatedAdventureLocation
    {
        public ulong ID { get; set; }
        public string Name { get; set; }

    }

    public class AssociatedAdventureType
    {
        public ulong ID { get; set; }
        public string Name { get; set; }
        public ulong ReviewCount { get; set; }
    }
}
