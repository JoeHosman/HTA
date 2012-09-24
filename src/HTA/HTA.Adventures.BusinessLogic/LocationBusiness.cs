using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HTA.Adventures.Models.Types;

namespace HTA.Adventures.BusinessLogic
{
    public class LocationBusiness : SpotBusiness
    {
        public bool Validate(AdventureLocation item, IList<ValidationResult> validationErrorResults)
        {
            if (null == validationErrorResults)
                validationErrorResults = new List<ValidationResult>();


            bool spotIsValidated = base.Validate(item, validationErrorResults);

            if (null == item || null == item.Region )
            {
                validationErrorResults.Add(new ValidationResult("A Region is required", new[] { "Region" }));
            }


            return validationErrorResults.Count == 0;
        }

    }
}