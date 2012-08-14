using HTA.Adventures.Models;
using HTA.Adventures.Models.Types;
using ServiceStack.ServiceInterface;

namespace HTA.Adventures.API.ServiceInterface
{
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
    public class AdventureRegionService : RestServiceBase<AdventureRegion>
    {
        public IAdventureRegionRepository AdventureRegionRepository { get; set; }

        public override object OnGet(AdventureRegion request)
        {
            if (!string.IsNullOrEmpty(request.Id))
            {
                return AdventureRegionRepository.GetAdventureRegion(request.Id);
            }
            return AdventureRegionRepository.GetAdventureRegions();
        }


        public override object OnPost(AdventureRegion request)
        {
            return AdventureRegionRepository.SaveAdventureRegion(request);
        }
    }
}