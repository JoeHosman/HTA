using System.Collections.Generic;
using HTA.Adventures.Models.Types;

namespace HTA.Adventures.Models
{
    public interface IAdventureTypeTemplateRepository
    {
        IList<AdventureTypeTemplate> GetTypeTemplateList();
        AdventureTypeTemplate GetTypeTemplate(string id);
        AdventureTypeTemplate SaveTypeTemplate(AdventureTypeTemplate template);
    }
}