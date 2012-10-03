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
    public class AdventureRegionService : RestServiceBase<Region>
    {
        public IAdventureRegionRepository AdventureRegionRepository { get; set; }
        public IAdventureLocationRepository AdventureLocationRepository { get; set; }

        public override object OnGet(Region request)
        {
            var response = new AdventureRegionGetResponse(request);
            if (!string.IsNullOrEmpty(request.Id))
            {
                var region = AdventureRegionRepository.GetAdventureRegion(request.Id);
                response.AdventureRegions.Add(region);
            }
            else
                response.AdventureRegions = AdventureRegionRepository.GetAdventureRegions();

            return response;
        }


        public override object OnPost(Region request)
        {
            var response = new AdventureRegionSaveResponse(request);

            using (var regionBusiness = new RegionBusiness())
            {
                IList<ValidationResult> validationErrorResults = new List<ValidationResult>();

                if (regionBusiness.Validate(request, validationErrorResults))
                {
                    var region = AdventureRegionRepository.SaveAdventureRegion(request);

                    response.AdventureRegion = region;

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