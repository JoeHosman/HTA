using HTA.Adventures.Models;
using HTA.Adventures.Models.Types;
using ServiceStack.ServiceInterface;

namespace HTA.Adventures.API.ServiceInterface
{
    public class AdventureTypeDataCardService : RestServiceBase<AdventureTypeDataCards>
    {
        public IAdventureTypeRepository AdventureTypeRepository { get; set; }
        public override object OnGet(AdventureTypeDataCards request)
        {
            var response = new AdventureTypeDataCardsResponse
                               {Id = request.Id, DataCards = AdventureTypeRepository.GetTypeDataCards(request.Id)};

            return response;
        }
    }
}