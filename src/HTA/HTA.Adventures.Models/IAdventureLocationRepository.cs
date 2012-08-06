using System;
using System.Collections.Generic;
using System.Text;
using HTA.Adventures.Models.Types;
using Newtonsoft.Json;

namespace HTA.Adventures.Models
{
    public interface IAdventureLocationRepository
    {
        void Add(AdventureLocation location);
        List<AdventureLocation> GetNearBy(Geo geoLocation, GeoRange geoRange);
    }
}
