using System.Collections.Generic;
using HTA.Adventures.Data.ModelValidation;
using HTA.Adventures.Models;
using HTA.Adventures.Models.Types;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceInterface.ServiceModel;

namespace HTA.Adventures.API.ServiceInterface
{
    public class AdventureReviewService : RestServiceBase<AdventureReview>
    {
        public IAdventureReviewRepository AdventureReviewRepository { get; set; }

        public override object OnPost(AdventureReview request)
        {
            var response = new AdventureReviewResponse(request);
            var validator = new GenericValidator<AdventureReview>();

            var results = validator.Validate(request);

            if (results.Count == 0)
            {
                response.Review = AdventureReviewRepository.SaveAdventureReview(request);

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

        public override object OnGet(AdventureReview request)
        {
            if (!string.IsNullOrEmpty(request.Id))
            {
                return AdventureReviewRepository.GetAdventureReviewById(request.Id);
            }
            return AdventureReviewRepository.GetAdventureReviews();
        }
    }
}