using System.Collections.Generic;
using HTA.Adventures.Models;
using HTA.Adventures.Models.Types;
using ServiceStack.ServiceClient.Web;

namespace HTA.Website.MVC.Example.API
{
    internal class APIAdventureTypeProxy : IAdventureTypeRepository
    {
        private readonly JsonServiceClient _client;

        public APIAdventureTypeProxy()
        {
            _client = new JsonServiceClient("http://localhost:10768/");

        }

        #region Implementation of IAdventureTypeRepository

        public IList<AdventureType> GetAdventureTypes()
        {
            var list = _client.Get<IList<AdventureType>>("/Adventure/Types");
            return list;
        }

        public AdventureType GetAdventureType(string id)
        {
            var adventureType = _client.Get<AdventureType>("/Adventure/Types/" + id);
            return adventureType;
        }

        public AdventureType SaveAdventureType(AdventureType adventuretype)
        {
            //lock (this)
            //{
            //    adventuretype.Id = (_stuff.Count + 1).ToString();
            //    _stuff.Add(adventuretype);
            //}
            adventuretype = _client.Post<AdventureType>("/Adventure/Types/", adventuretype);
            return adventuretype;
        }

        public IList<AdventureDataCard> GetTypeDataCards(string id)
        {
            var dataCards = _client.Get<IList<AdventureDataCard>>("/Adventure/Type/DataCards/" + id);
            return dataCards;
        }

        #endregion
    }
}