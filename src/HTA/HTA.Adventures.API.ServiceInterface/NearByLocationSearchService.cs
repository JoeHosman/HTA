using System.Collections.Generic;
using System.Linq;
using HTA.Adventures.Models;
using HTA.Adventures.Models.Types;
using HTA.Adventures.Models.Types.Responses;
using Nest;
using ServiceStack.ServiceInterface;

namespace HTA.Adventures.API.ServiceInterface
{
    public class NearByLocationSearchService : RestServiceBase<NearByAdventureLocations>
    {
        public IAdventureLocationSearchRepository LocationSearchRepository { get; set; }
        public override object OnGet(NearByAdventureLocations request)
        {
            var response = new NearBySearchResponse(request);

            if (!string.IsNullOrEmpty(request.LatLon))
            {
                var values = request.SplitLatLon();
                request.Lat = values[0];
                request.Lon = values[1];
            }

            response.Result = LocationSearchRepository.GetNearByAdventureLocations(request);


            return response;
        }
    }
}