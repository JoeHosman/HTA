using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HTA.Adventures.Models.Types;

namespace HTA.Adventures.BusinessLogic
{
    public class ReviewBusiness : IDisposable
    {
        public bool Validate(AdventureReview item, IList<ValidationResult> validationErrorResults)
        {
            if (item != null)
            {
                if (string.IsNullOrEmpty(item.Name))
                    validationErrorResults.Add(new ValidationResult("A Name is required", new[] { "ReviewName" }));
            }
            else
                validationErrorResults.Add(new ValidationResult("A non-null object is required.", new[] { "Review" }));
            return validationErrorResults.Count == 0;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}