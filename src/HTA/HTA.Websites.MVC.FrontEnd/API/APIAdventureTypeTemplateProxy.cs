using System.Collections.Generic;
using HTA.Adventures.Models;
using HTA.Adventures.Models.Types;
using ServiceStack.ServiceClient.Web;

namespace HTA.Websites.MVC.FrontEnd.API
{
    internal class APIAdventureTypeTemplateProxy : IAdventureTypeTemplateRepository
    {
        private readonly JsonServiceClient _client;

        public APIAdventureTypeTemplateProxy()
        {
            _client = new JsonServiceClient("http://localhost:10768/");

        }

        public IList<AdventureTypeTemplate> GetTypeTemplateList()
        {
            var list = _client.Get<IList<AdventureTypeTemplate>>("/Adventure/TypeTemplates");
            return list;
        }

        public AdventureTypeTemplate GetTypeTemplate(string id)
        {
            var adventureType = _client.Get<AdventureTypeTemplate>("/Adventure/TypeTemplates/" + id);
            return adventureType;
        }

        public AdventureTypeTemplate SaveTypeTemplate(AdventureTypeTemplate template)
        {
            template = _client.Post<AdventureTypeTemplate>("/Adventure/TypeTemplates/", template);
            return template;
        }
    }
}