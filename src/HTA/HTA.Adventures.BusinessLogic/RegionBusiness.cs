using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HTA.Adventures.Models.Types;

namespace HTA.Adventures.BusinessLogic
{
    public class RegionBusiness : SpotBusiness
    {
        public bool Validate(Region item, IList<ValidationResult> validationErrorResults)
        {
            if (null == validationErrorResults)
                validationErrorResults = new List<ValidationResult>();

            bool spotIsValidated = base.Validate(item, validationErrorResults);

            return validationErrorResults.Count == 0;
        }
    }
}