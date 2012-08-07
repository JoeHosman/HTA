using System.Collections.Generic;
using HTA.Adventures.Models.Types;

namespace HTA.Website.MVC.Example.Models
{
    public class AdventureReviewModel
    {

        public AdventureReview Review { get; set; }
        public IEnumerable<AdventureType> SelectableTypes { get; set; }

        public int DataCardCount { get; set; }
        public string AdventureTypeId { get; set; }
    }
}