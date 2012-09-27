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
    public class AdventureDataCardService : RestServiceBase<AdventureDataCard>
    {
        public IAdventureDataCardRepository AdventureDataCardRepository { get; set; }

        public override object OnGet(AdventureDataCard request)
        {
            var response = new AdventureDataCardGetResponse(request);

            if (null != request.AdventureReview && !string.IsNullOrEmpty(request.AdventureReview.Id))
            {
                response.DataCards = AdventureDataCardRepository.GetAdventureDataCardsByReviewId(request.AdventureReview.Id);
            }
            else if (!string.IsNullOrEmpty(request.Id))
            {
                var dataCard = AdventureDataCardRepository.GetAdventureDataCardById(request.Id);
                response.DataCards.Add(dataCard);
            }



            return response;
        }


        public override object OnPost(AdventureDataCard request)
        {
            var response = new AdventureDataCardSaveResponse(request);

            using (var regionBusiness = new AdventureDataCardBusiness())
            {
                IList<ValidationResult> validationErrorResults = new List<ValidationResult>();

                if (regionBusiness.Validate(request, validationErrorResults))
                {
                    var dataCard = AdventureDataCardRepository.SaveAdventureDataCard(request);

                    response.DataCard = dataCard;
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