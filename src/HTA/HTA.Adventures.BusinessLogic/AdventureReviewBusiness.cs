using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HTA.Adventures.Models.Types;

namespace HTA.Adventures.BusinessLogic
{
    public class AdventureReviewBusiness : IDisposable
    {
        public bool Validate(AdventureReview item, IList<ValidationResult> validationErrorResults)
        {
            if (null == validationErrorResults)
                validationErrorResults = new List<ValidationResult>();

            if (null == item)
                validationErrorResults.Add(new ValidationResult("A non-null object is required.", new[] { "AdventureReview" }));
            else
            {
                if (string.IsNullOrEmpty(item.AdventureName))
                    validationErrorResults.Add(new ValidationResult("A Name is required", new[] { "AdventureReviewName" }));

                if (null == item.AdventureType || string.IsNullOrEmpty(item.AdventureType.Id))
                    validationErrorResults.Add(new ValidationResult("A Adventure Type is required", new[] { "AdventureReviewAdventureType" }));

                using (var locationBusiness = new LocationBusiness())
                {
                    if (null == item.AdventureLocation)
                        validationErrorResults.Add(new ValidationResult("A Location is required", new[] { "AdventureReviewLocation" }));
                    else if (!locationBusiness.Validate(item.AdventureLocation, validationErrorResults))
                    {
                        validationErrorResults.Add(new ValidationResult("A Valid Location is required", new[] { "AdventureReviewLocation" }));
                    }
                }

                if (null == item.AdventureDuration || item.AdventureDuration <= new TimeSpan())
                    validationErrorResults.Add(new ValidationResult("A Duration Time span is required", new[] { "AdventureReviewDuriation" }));

                if (null == item.AdventureDate)
                {
                    validationErrorResults.Add(new ValidationResult("A Adventure Date is required", new[] { "AdventureReviewAdventureDate" }));
                }


            }
            return validationErrorResults.Count == 0;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}