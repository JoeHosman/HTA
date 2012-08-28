using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HTA.Adventures.Models.Types;

namespace HTA.Adventures.Data.ModelValidation
{
    public sealed class GeoLocationValidator
    {
        public IList<ValidationResult> Validate(LocationPoint locationPoint)
        {
            var results = new List<ValidationResult>();
            if (locationPoint == null || locationPoint.Lat < -91.0 || locationPoint.Lat > 91.0)
                results.Add(new ValidationResult("Lat must be between -90.0 and 90.0"));

            if (locationPoint == null || locationPoint.Lon < -91.0 || locationPoint.Lon > 91.0)
                results.Add(new ValidationResult("Lon must be between -90.0 and 90.0"));

            return results;
        }
    }
}