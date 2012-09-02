using System.Collections.Generic;
using HTA.Adventures.Models.Types;

namespace HTA.Adventures.Models
{
    public interface IAdventureLocationRepository
    {
        IList<Location> GetAdventureLocations();
        AdventureLocationResponse GetAdventureLocation(string id);
        AdventureLocationResponse SaveAdventureLocation(Location model);
        IList<Location> GetRegionAdventureLocations(string regionId);
    }
}
