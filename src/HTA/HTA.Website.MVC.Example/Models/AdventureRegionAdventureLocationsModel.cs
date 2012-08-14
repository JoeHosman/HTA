using System.Collections.Generic;
using HTA.Adventures.Models.Types;

namespace HTA.Website.MVC.Example.Models
{
    public class AdventureRegionAdventureLocationsModel
    {
        public AdventureRegion Region { get; set; }
        public IEnumerable<string> SelectedTemplates { get; set; }
        public IList<AdventureLocation> Locations { get; set; }
    }
}