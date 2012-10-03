using HTA.Adventures.Models;
using HTA.Adventures.Models.Types;
using HTA.Adventures.Models.Types.Responses;
using ServiceStack.ServiceInterface;

namespace HTA.Adventures.API.ServiceInterface
{
    public class NearByAdventureLocationSearchService : RestServiceBase<NearByAdventureLocationSearch>
    {
        public IAdventureLocationSearchRepository AdventureLocationSearchRepository { get; set; }

        public override object OnGet(NearByAdventureLocationSearch request)
        {
            var response = new NearByAdventureLocationSearchResponse(request);

            response.NearByAdventureLocations = AdventureLocationSearchRepository.GetNearByAdventureLocations(request.Point, request.Range);

            return response;
        }
    }
}