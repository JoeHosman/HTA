using HTA.Adventures.Models;
using HTA.Adventures.Models.Types;
using ServiceStack.ServiceInterface;

namespace HTA.Adventures.API.ServiceInterface
{
    public class AdventureRegionAdventureLocationsService : RestServiceBase<AdventureRegionAdventureLocationsRequest>
    {
        public IAdventureLocationRepository AdventureLocationRepository { get; set; }
        public override object OnGet(AdventureRegionAdventureLocationsRequest request)
        {
            return AdventureLocationRepository.GetRegionAdventureLocations(request.RegionId);
        }

    }
}