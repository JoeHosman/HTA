using System.Collections.Generic;
using HTA.Adventures.Models;
using HTA.Adventures.Models.Types;

namespace HTA.Website.MVC.Example.API
{
    internal class APIAdventureRegionProxy : APIProxyBase, IAdventureRegionRepository
    {
        #region Implementation of IAdventureRegionRepository

        public IList<Region> GetAdventureRegions()
        {
            var list = Client.Get<IList<Region>>("/Adventure/Regions");
            return list;
        }

        public Region GetAdventureRegion(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;

            var review = Client.Get<Region>("/Adventure/Regions/" + id);
            return review;
        }

        public Region SaveAdventureRegion(Region region)
        {
            var review = Client.Post<Region>("/Adventure/Regions/", region);
            return review;
        }

        #endregion
    }
}