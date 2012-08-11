using System.Collections.Generic;
using System.Linq;
using HTA.Adventures.Models.Types;

namespace HTA.Websites.MVC.FrontEnd.Models
{
    public class AdventureTypeModel
    {
        public AdventureTypeModel()
            : this(new AdventureType())
        {

        }
        public AdventureTypeModel(AdventureType adventuretype)
        {
            AdventureType = adventuretype;
            SelectedTemplates = AdventureType.DataCardTemplates.Select(a => a.Id).ToList();
        }

        public AdventureType AdventureType { get; set; }
        public IEnumerable<string> SelectedTemplates { get; set; }
        public IEnumerable<AdventureTypeTemplate> TemplateList { get; set; }
    }
}