using System.Collections.Generic;
using HTA.Adventures.Models.Types;
using HTA.Adventures.Models.Types.Responses;

namespace HTA.Adventures.Models
{
    public interface IAdventureLocationRepository
    {
        IList<AdventureLocation> GetAdventureLocations();
        AdventureLocation GetAdventureLocation(string id);
        AdventureLocation SaveAdventureLocation(AdventureLocation model);
        IList<AdventureLocation> GetRegionAdventureLocations(string regionId);
    }
}
