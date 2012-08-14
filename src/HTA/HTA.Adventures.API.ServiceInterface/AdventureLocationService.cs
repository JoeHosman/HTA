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

    public class AdventureLocationService : RestServiceBase<AdventureLocation>
    {
        public IAdventureLocationRepository AdventureLocationRepository { get; set; }
        public override object OnGet(AdventureLocation request)
        {
            if (!string.IsNullOrEmpty(request.Id))
            {
                return AdventureLocationRepository.GetAdventureLocation(request.Id);
            }
            return AdventureLocationRepository.GetAdventureLocations();
        }

        public override object OnPost(AdventureLocation request)
        {
            return AdventureLocationRepository.SaveAdventureReview(request);
        }
    }
}