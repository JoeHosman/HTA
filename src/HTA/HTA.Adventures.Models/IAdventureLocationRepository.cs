using System.Collections.Generic;
using HTA.Adventures.Models.Types;

namespace HTA.Adventures.Models
{
    public interface IAdventureLocationRepository
    {
        void Add(AdventureLocation location);
        List<AdventureLocation> GetNearBy(Geo geoLocation, GeoRange geoRange);
    }
}
