using System.Collections.Generic;
using HTA.Adventures.Models.Types;

namespace HTA.Adventures.Models
{
    public interface IAdventureRegionRepository
    {
        IList<Region> GetAdventureRegions();
        Region GetAdventureRegion(string id);
        Region SaveAdventureRegion(Region region);
    }
}