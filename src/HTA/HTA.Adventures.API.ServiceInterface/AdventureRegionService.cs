using System.Collections.Generic;
using HTA.Adventures.Data.ModelValidation;
using HTA.Adventures.Models;
using HTA.Adventures.Models.Types;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceInterface.ServiceModel;

namespace HTA.Adventures.API.ServiceInterface
{
    public class AdventureRegionService : RestServiceBase<AdventureRegion>
    {
        public IAdventureRegionRepository AdventureRegionRepository { get; set; }
        public IAdventureLocationRepository AdventureLocationRepository { get; set; }

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
            var response = new AdventureRegionResponse(request);

            var validator = new AdventureSpotValidator();
            var results = validator.Validate(response.Request);

            if (results.Count == 0)
            {
                response.Region = AdventureRegionRepository.SaveAdventureRegion(request);

                response.Locations = AdventureLocationRepository.GetRegionAdventureLocations(response.Region.Id);

                if (response.Locations.Count == 0)
                {
                    var location = AdventureLocationRepository.SaveAdventureLocation(response.Region.CreateLocation()); ;
                    
                    
                    response.Locations.Add(location);
                }

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