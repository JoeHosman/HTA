using System.Collections.Generic;
using HTA.Adventures.Models;
using HTA.Adventures.Models.Types;

namespace HTA.Website.MVC.Example.API
{
    internal class APIAdventureLocationProxy : APIProxyBase, IAdventureLocationRepository
    {
        public IList<AdventureLocation> GetAdventureLocations()
        {
            var list = Client.Get<List<AdventureLocation>>("/Adventure/Locations");
            return list;
        }

        public AdventureLocation GetAdventureLocation(string id)
        {
            var adventureLocation = Client.Get<AdventureLocation>("/Adventure/Locations/"+ id);
            return adventureLocation;
        }

        public AdventureLocation SaveAdventureLocation(AdventureLocation model)
        {
            var Location = Client.Post<AdventureLocation>("/Adventure/Locations/" + model.Id, model);
            return Location;
        }

        public IList<AdventureLocation> GetRegionAdventureLocations(string regionId)
        {
            var locations = Client.Get<List<AdventureLocation>>("/Adventure/Region/Locations/" + regionId);
            return locations;
        }
    }
}