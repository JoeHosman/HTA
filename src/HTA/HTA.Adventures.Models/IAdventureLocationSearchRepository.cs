using System.Collections.Generic;
using HTA.Adventures.Models.Types;

namespace HTA.Adventures.Models
{
    public interface IAdventureLocationSearchRepository
    {
        List<AdventureLocation> GetNearByAdventureLocations(GeoPoint point, GeoRange range);
    }
}