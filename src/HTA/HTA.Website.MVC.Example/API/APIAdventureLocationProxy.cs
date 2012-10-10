using System.Collections.Generic;
using HTA.Adventures.Models;
using HTA.Adventures.Models.Types;
using HTA.Adventures.Models.Types.Responses;

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

            var response = Client.Get<AdventureLocationGetResponse>("/Adventure/Locations/" + id);

            return response.AdventureLocations[0];
        }

        public AdventureLocation SaveAdventureLocation(AdventureLocation model)
        {

            var response = Client.Post<AdventureLocationSaveResponse>("/Adventure/Locations/" + model.Id, model);

            return response.AdventureLocation;
        }

        public IList<AdventureLocation> GetRegionAdventureLocations(string regionId)
        {
            var locations = Client.Get<List<AdventureLocation>>("/Adventure/Region/Locations/" + regionId);
            return locations;
        }
    }
}