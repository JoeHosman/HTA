using System.Collections.Generic;
using HTA.Adventures.Models;
using HTA.Adventures.Models.Types;

namespace HTA.Website.MVC.Example.API
{
    internal class APIAdventureLocationProxy : APIProxyBase, IAdventureLocationRepository
    {
        public IList<Location> GetAdventureLocations()
        {
            var list = Client.Get<List<Location>>("/Adventure/Locations");
            return list;
        }

        public AdventureLocationResponse GetAdventureLocation(string id)
        {

            var response = Client.Get<AdventureLocationResponse>("/Adventure/Locations/" + id);

            return response;
        }

        public AdventureLocationResponse SaveAdventureLocation(Location model)
        {

            var response = Client.Post<AdventureLocationResponse>("/Adventure/Locations/" + model.Id, model);

            return response;
        }

        public IList<Location> GetRegionAdventureLocations(string regionId)
        {
            var locations = Client.Get<List<Location>>("/Adventure/Region/Locations/" + regionId);
            return locations;
        }
    }
}