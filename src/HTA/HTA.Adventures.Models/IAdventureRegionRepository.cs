using System.Collections.Generic;
using HTA.Adventures.Models.Types;

namespace HTA.Adventures.Models
{
    public interface IAdventureRegionRepository
    {
        IList<AdventureRegion> GetAdventureRegions();
        AdventureRegion GetAdventureRegion(string id);
        AdventureRegion SaveAdventureRegion(AdventureRegion adventureRegion);
    }
}