using System.Collections.Generic;
using HTA.Adventures.Models;
using HTA.Adventures.Models.Types;
using ServiceStack.ServiceClient.Web;

namespace HTA.Website.MVC.Example.API
{
    internal class APIAdventureTypeTemplateProxy :APIProxyBase, IAdventureTypeTemplateRepository
    {

        public IList<AdventureTypeTemplate> GetTypeTemplateList()
        {
            var list = Client.Get<IList<AdventureTypeTemplate>>("/Adventure/TypeTemplates");
            return list;
        }

        public AdventureTypeTemplate GetTypeTemplate(string id)
        {
            var adventureType = Client.Get<AdventureTypeTemplate>("/Adventure/TypeTemplates/" + id);
            return adventureType;
        }

        public AdventureTypeTemplate SaveTypeTemplate(AdventureTypeTemplate template)
        {
            template = Client.Post<AdventureTypeTemplate>("/Adventure/TypeTemplates/", template);
            return template;
        }
    }
}