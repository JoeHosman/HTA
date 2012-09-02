using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HTA.Adventures.BusinessLogic;
using HTA.Adventures.Models;
using HTA.Adventures.Models.Types;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceInterface.ServiceModel;

namespace HTA.Adventures.API.ServiceInterface
{
    public class AdventureRegionService : RestServiceBase<Region>
    {
        public IAdventureRegionRepository AdventureRegionRepository { get; set; }
        public IAdventureLocationRepository AdventureLocationRepository { get; set; }

        public override object OnGet(Region request)
        {
            if (!string.IsNullOrEmpty(request.Id))
            {
                return AdventureRegionRepository.GetAdventureRegion(request.Id);
            }
            return AdventureRegionRepository.GetAdventureRegions();
        }


        public override object OnPost(Region request)
        {
            var response = new AdventureRegionResponse(request);

            using (var regionBusiness = new RegionBusiness())
            {
                IList<ValidationResult> validationErrorResults = new List<ValidationResult>();
               
                if (regionBusiness.Validate(request, validationErrorResults))
                {
                    response.Region = AdventureRegionRepository.SaveAdventureRegion(request);

                    response.Locations = AdventureLocationRepository.GetRegionAdventureLocations(response.Region.Id);

                    if (response.Locations.Count == 0)
                    {
                        var locationResponse = AdventureLocationRepository.SaveAdventureLocation(response.Region.CreateLocation()); ;


                        response.Locations.Add(locationResponse.Location);
                    }

                    response.ResponseStatus = new ResponseStatus();
                }
                else
                {
                    response.ResponseStatus = new ResponseStatus("Invalid", string.Format("'{0}' errors prevents this from valid.", validationErrorResults.Count));
                    ErrorUtility.TransformResponseErrors(response.ResponseStatus.Errors, validationErrorResults);
                }
            }

            return response;

        }
    }
}