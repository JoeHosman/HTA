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
    public class AdventureReviewService : RestServiceBase<AdventureReview>
    {
        public IAdventureReviewRepository AdventureReviewRepository { get; set; }

        public override object OnPost(AdventureReview request)
        {
            var response = new AdventureReviewSaveResponse(request);
            
            using (var reviewBusiness = new AdventureReviewBusiness())
            {
                IList<ValidationResult> validationErrorResults = new List<ValidationResult>();
                if (reviewBusiness.Validate(request, validationErrorResults))
                {
                    response.AdventureReview = AdventureReviewRepository.SaveAdventureReview(request);

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




        public override object OnGet(AdventureReview request)
        {
            var response = new AdventureReviewGetResponse(request);

            if (!string.IsNullOrEmpty(request.Id))
            {
                response.AdventureReviews.Add(AdventureReviewRepository.GetAdventureReviewById(request.Id));
            }
            else
                response.AdventureReviews = AdventureReviewRepository.GetAdventureReviews();
            return response;
        }
    }
}