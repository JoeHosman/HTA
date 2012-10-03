using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using HTA.Adventures.Models.Types;

namespace HTA.Website.MVC.Example.Models
{
    [DataContract]
    public class AdventureLocationModel
    {
        [DataMember]
        public AdventureLocation AdventureLocation { get; set; }

        [DataMember]
        public IList<Region> SelectableAdventureRegions { get; set; }
    }
}