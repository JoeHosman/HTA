using System.Collections.Generic;
using HTA.Adventures.Models.Types;

namespace HTA.Website.MVC.Example.Models
{
    public class AdventureRegionAdventureLocationsModel
    {
        public Region Region { get; set; }
        public IEnumerable<string> SelectedLocations { get; set; }
        public IList<Location> Locations { get; set; }
    }
}