using System.Collections.Generic;
using HTA.Adventures.Models;
using HTA.Adventures.Models.Types;
using ServiceStack.ServiceClient.Web;

namespace HTA.Website.MVC.Example.API
{
    internal class APIAdventureTypeProxy :APIProxyBase, IAdventureTypeRepository
    {

        #region Implementation of IAdventureTypeRepository

        public IList<AdventureType> GetAdventureTypes()
        {
            var list = Client.Get<List<AdventureType>>("/Adventure/Types");
            return list;
        }

        public AdventureType GetAdventureType(string id)
        {
            var adventureType = Client.Get<AdventureType>("/Adventure/Types/" + id);
            return adventureType;
        }

        public AdventureType SaveAdventureType(AdventureType adventuretype)
        {
            //lock (this)
            //{
            //    adventuretype.Id = (_stuff.Count + 1).ToString();
            //    _stuff.Add(adventuretype);
            //}
            adventuretype = Client.Post<AdventureType>("/Adventure/Types/", adventuretype);
            return adventuretype;
        }

        public IList<AdventureDataCard> GetTypeDataCards(string typeId)
        {
            var dataCards = Client.Get<IList<AdventureDataCard>>("/Adventure/Type/DataCards/" + typeId);
            return dataCards;
        }

        #endregion
    }
}