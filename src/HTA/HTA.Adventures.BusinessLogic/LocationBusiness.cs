using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HTA.Adventures.Models.Types;

namespace HTA.Adventures.BusinessLogic
{
    public class RegionBusiness : SpotBusiness
    {
        public bool Validate(Region item, IList<ValidationResult> validationErrorResults)
        {
            bool spotIsValidated = base.Validate(item, validationErrorResults);

            return validationErrorResults.Count == 0;
        }
    }

    public class LocationBusiness : SpotBusiness
    {
        public bool Validate(Spot item, IList<ValidationResult> validationErrorResults)
        {
            bool spotIsValidated = base.Validate(item, validationErrorResults);

            return validationErrorResults.Count == 0;
        }

    }
}