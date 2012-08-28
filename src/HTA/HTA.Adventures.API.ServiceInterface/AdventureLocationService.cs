using System.Collections.Generic;
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

    public class AdventureLocationService : RestServiceBase<AdventureLocation>
    {
        public IAdventureLocationRepository AdventureLocationRepository { get; set; }
        public IAdventureRegionRepository AdventureRegionRepository { get; set; }

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
            var response = new AdventureLocationResponse(request);

            var validator = new AdventureSpotValidator();
            var results = validator.Validate(response.Request);

            if (results.Count == 0)
            {
                if (string.IsNullOrEmpty(request.AdventureRegion.Id))
                {
                    response.Region =
                        AdventureRegionRepository.SaveAdventureRegion(request.AdventureRegion);
                }
                else
                {
                    // attempt to get the right region based off Id, if none are found, create a new one.
                    response.Region = AdventureRegionRepository.GetAdventureRegion(request.AdventureRegion.Id) ??
                                      AdventureRegionRepository.SaveAdventureRegion(request.AdventureRegion);
                }

                request.AdventureRegion = response.Region;

                response.Location = AdventureLocationRepository.SaveAdventureLocation(request);

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