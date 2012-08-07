using HTA.Adventures.Models;
using HTA.Adventures.Models.Types;
using ServiceStack.ServiceInterface;

namespace HTA.Adventures.API.ServiceInterface
{
    public class AdventureTypeTemplateService : RestServiceBase<AdventureTypeTemplate>
    {
        public IAdventureTypeTemplateRepository AdventureTypeRepository { get; set; }

        public override object OnPost(AdventureTypeTemplate request)
        {
            return AdventureTypeRepository.SaveTypeTemplate(request);
        }

        private AdventureTypeTemplate GetTemplate(string id)
        {
            return AdventureTypeRepository.GetTypeTemplate(id);
        }

        public override object OnGet(AdventureTypeTemplate request)
        {

            if (!string.IsNullOrEmpty(request.Id))
            {
                return GetTemplate(request.Id);
            }
            return AdventureTypeRepository.GetTypeTemplateList();

        }
    }
}