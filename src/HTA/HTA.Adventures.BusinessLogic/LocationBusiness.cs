using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HTA.Adventures.Models.Types;

namespace HTA.Adventures.BusinessLogic
{
    public class LocationBusiness : SpotBusiness
    {
        public bool Validate(Location item, IList<ValidationResult> validationErrorResults)
        {
            if (item != null)
            {
                bool spotIsValidated = base.Validate(item, validationErrorResults);

                if (item.Region == null || string.IsNullOrEmpty(item.Region.Id))
                {
                    validationErrorResults.Add(new ValidationResult("A Region is required", new[] { "Region" }));
                }
            }
            else
                validationErrorResults.Add(new ValidationResult("A non-null object is required.", new[] { "Location" }));

            return validationErrorResults.Count == 0;
        }

    }
}