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
    public class AdventureTypeService : RestServiceBase<AdventureType>
    {
        public IAdventureTypeRepository AdventureTypeRepository { get; set; }

        public override object OnPost(AdventureType request)
        {
            var response = new AdventureTypeSaveResponse(request);

            using (var business = new AdventureTypeBusiness())
            {
                IList<ValidationResult> validationErrorResults = new List<ValidationResult>();

                if (business.Validate(request, validationErrorResults))
                {
                    response.AdventureType = AdventureTypeRepository.SaveAdventureType(request);
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


        public override object OnGet(AdventureType request)
        {
            var response = new AdventureTypeGetResponse(request);

            if (string.IsNullOrEmpty(request.Id))
            {
                response.AdventureTypes = AdventureTypeRepository.GetAdventureTypes();
            }
            else
            {
                var result = AdventureTypeRepository.GetAdventureType(request.Id);
                response.AdventureTypes.Add(result);
            }
            return response;
        }

        public override object OnDelete(AdventureType request)
        {
            var response = new AdventureTypeDeleteResponse(request);

            return response;
        }
    }
}