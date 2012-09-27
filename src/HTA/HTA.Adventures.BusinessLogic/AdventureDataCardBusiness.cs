using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HTA.Adventures.Models.Types;

namespace HTA.Adventures.BusinessLogic
{
    public class AdventureDataCardBusiness : IDisposable
    {
        public bool Validate(AdventureDataCard item, IList<ValidationResult> validationErrorResults)
        {
            if (null == validationErrorResults)
                validationErrorResults = new List<ValidationResult>();

            if (null == item)
            {
                validationErrorResults.Add(new ValidationResult("A non-null object is required.", new[] { "AdventureDataCard" }));
            }
            else
            {
                if (string.IsNullOrEmpty(item.Title))
                    validationErrorResults.Add(new ValidationResult("A Title is required", new[] { "DataCardTitle" }));
                if (string.IsNullOrEmpty(item.Body))
                    validationErrorResults.Add(new ValidationResult("A Description is required", new[] { "DataCardBody" }));
               
            }

            return validationErrorResults.Count == 0;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}