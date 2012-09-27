using System.Collections.Generic;
using HTA.Adventures.Models;
using HTA.Adventures.Models.Types;
using HTA.Adventures.Models.Types.Responses;
using ServiceStack.ServiceClient.Web;

namespace HTA.Website.MVC.Example.API
{
    internal class APIAdventureTypeProxy : APIProxyBase, IAdventureTypeRepository
    {

        #region Implementation of IAdventureTypeRepository

        public IList<AdventureType> GetAdventureTypes()
        {
            var response = Client.Get<AdventureTypeGetResponse>("/Adventure/Types");
            return response.AdventureTypes;
        }

        public AdventureType GetAdventureType(string id)
        {
            var response = Client.Get<AdventureTypeGetResponse>("/Adventure/Types/" + id);
            return response.AdventureTypes[0];
        }

        public AdventureType SaveAdventureType(AdventureType adventuretype)
        {

            var response = Client.Post<AdventureTypeSaveResponse>("/Adventure/Types/", adventuretype);
            return response.AdventureType;
        }

        public IList<AdventureDataCard> GetTypeDataCards(string typeId)
        {
            var dataCards = Client.Get<IList<AdventureDataCard>>("/Adventure/Type/DataCards/" + typeId);
            return dataCards;
        }

        #endregion
    }
}