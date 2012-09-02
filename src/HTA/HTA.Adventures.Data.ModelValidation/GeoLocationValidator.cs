using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HTA.Adventures.Models.Types;

namespace HTA.Adventures.Data.ModelValidation
{
    public sealed class GeoLocationValidator
    {
        public IList<ValidationResult> Validate(GeoPoint geoPoint)
        {
            var results = new List<ValidationResult>();
            if (geoPoint == null || geoPoint.Lat < -90.0 || geoPoint.Lat > 90.0)
                results.Add(new ValidationResult("Lat must be between -90.0 and 90.0"));

            if (geoPoint == null || geoPoint.Lon < -180.0 || geoPoint.Lon > 180.0)
                results.Add(new ValidationResult("Lon must be between -90.0 and 90.0"));

            return results;
        }
    }
}