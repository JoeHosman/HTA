using System.Collections.Generic;
using HTA.Adventures.BusinessLogic;
using HTA.Adventures.Data.ModelValidation;
using HTA.Adventures.Models;
using HTA.Adventures.Models.Types;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceInterface.ServiceModel;

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

    public class AdventureLocationService : RestServiceBase<Location>
    {
        public IAdventureLocationRepository AdventureLocationRepository { get; set; }
        public IAdventureRegionRepository AdventureRegionRepository { get; set; }

        public override object OnGet(Location request)
        {
            if (!string.IsNullOrEmpty(request.Id))
            {
                return AdventureLocationRepository.GetAdventureLocation(request.Id);
            }
            return AdventureLocationRepository.GetAdventureLocations();
        }

        public override object OnPost(Location request)
        {
            var response = new AdventureLocationResponse(request);

            var spotBusiness = new SpotBusiness();
            
            var validator = new AdventureSpotValidator();
            var results = validator.Validate(response.Request);

            if (results.Count == 0)
            {
                if (string.IsNullOrEmpty(request.Region.Id))
                {
                    response.Region =
                        AdventureRegionRepository.SaveAdventureRegion(request.Region);
                }
                else
                {
                    // attempt to get the right region based off Id, if none are found, create a new one.
                    response.Region = AdventureRegionRepository.GetAdventureRegion(request.Region.Id) ??
                                      AdventureRegionRepository.SaveAdventureRegion(request.Region);
                }

                request.Region = response.Region;

                response.Location = AdventureLocationRepository.SaveAdventureLocation(request).Location;

                response.ResponseStatus = new ResponseStatus();
            }
            else
            {
                response.ResponseStatus = new ResponseStatus("Invalid", string.Format("'{0}' errors prevents this from valid.", results.Count));
                response.ResponseStatus.Errors = new List<ResponseError>();

                foreach (var validationResult in results)
                {
                    validationResult.MemberNames.GetEnumerator().MoveNext();
                    response.ResponseStatus.Errors.Add(new ResponseError
                    {
                        //FieldName = validationResult.MemberNames.GetEnumerator().Current,
                        Message = validationResult.ErrorMessage,
                        ErrorCode = "FailedValidation"
                    });
                }
            }

            return response;
        }
    }
}