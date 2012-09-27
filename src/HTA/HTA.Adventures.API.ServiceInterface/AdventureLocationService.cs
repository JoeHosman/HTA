using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HTA.Adventures.BusinessLogic;
using HTA.Adventures.Models;
using HTA.Adventures.Models.Types;
using HTA.Adventures.Models.Types.Responses;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceInterface.ServiceModel;

namespace HTA.Adventures.API.ServiceInterface
{
    public class AdventureLocationService : RestServiceBase<AdventureLocation>
    {
        public IAdventureLocationRepository AdventureLocationRepository { get; set; }
        public IAdventureRegionRepository AdventureRegionRepository { get; set; }

        public override object OnGet(AdventureLocation request)
        {
            var response = new AdventureLocationGetResponse(request);

            if (!string.IsNullOrEmpty(request.Id))
            {
                var location = AdventureLocationRepository.GetAdventureLocation(request.Id);
                response.AdventureLocations.Add(location);
            }
            else
            {
                response.AdventureLocations = AdventureLocationRepository.GetAdventureLocations();
            }
            return response;
        }

        public override object OnPost(AdventureLocation request)
        {
            var response = new AdventureLocationSaveResponse(request);
            //response.ResponseStatus = new ResponseStatus() { ErrorCode = "NULL" };

            using (var locationBusiness = new LocationBusiness())
            {
                IList<ValidationResult> validationErrorResults = new List<ValidationResult>();
                if (locationBusiness.Validate(request, validationErrorResults))
                {
                    response.AdventureLocation = AdventureLocationRepository.SaveAdventureLocation(request);

                    response.ResponseStatus = new ResponseStatus();
                }
                else
                {
                    response.ResponseStatus = new ResponseStatus("Invalid",
                                                                 string.Format(
                                                                     "'{0}' errors prevents this from valid.",
                                                                     validationErrorResults.Count));

                    ErrorUtility.TransformResponseErrors(response.ResponseStatus.Errors, validationErrorResults);
                }
            }

            return response;
        }
    }
}