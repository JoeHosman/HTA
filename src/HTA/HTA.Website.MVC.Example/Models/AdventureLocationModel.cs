using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HTA.Adventures.Models.Types;

namespace HTA.Website.MVC.Example.Models
{
    public class AdventureLocationModel
    {
        public AdventureLocation AdventureLocation { get; set; }

        public IList<AdventureRegion> SelectableAdventureRegions { get; set; }
    }
}