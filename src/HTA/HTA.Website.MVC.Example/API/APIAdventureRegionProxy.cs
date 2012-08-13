using System.Collections.Generic;
using HTA.Adventures.Models;
using HTA.Adventures.Models.Types;

namespace HTA.Website.MVC.Example.API
{
    internal class APIAdventureRegionProxy : APIProxyBase, IAdventureRegionRepository
    {
        #region Implementation of IAdventureRegionRepository

        public IList<AdventureRegion> GetAdventureRegions()
        {
            var list = Client.Get<IList<AdventureRegion>>("/Adventure/Regions");
            return list;
        }

        public AdventureRegion GetAdventureRegion(string id)
        {
            var review = Client.Get<AdventureRegion>("/Adventure/Regions/" + id);
            return review;
        }

        public AdventureRegion SaveAdventureRegion(AdventureRegion adventureRegion)
        {
            var review = Client.Post<AdventureRegion>("/Adventure/Regions/", adventureRegion);
            return review;
        }

        #endregion
    }
}