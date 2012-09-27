using System.Collections.Generic;
using HTA.Adventures.Models.Types;

namespace HTA.Adventures.Models
{
    public interface IAdventureLocationSearchRepository
    {
        List<Spot> GetNearByAdventureLocations(NearByAdventureLocations request);
    }
}