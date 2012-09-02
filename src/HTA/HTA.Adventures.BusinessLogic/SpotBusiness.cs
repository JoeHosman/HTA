using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using HTA.Adventures.Models.Types;

namespace HTA.Adventures.BusinessLogic
{
    public class SpotBusiness : IDisposable
    {
        public bool Validate(Location item)
        {
            IList<ValidationResult> validationErrorResults = new List<ValidationResult>();
            if (item != null)
            {
                if (string.IsNullOrEmpty(item.Name))
                    validationErrorResults.Add(new ValidationResult("A Name is required", new[] { "LocationName" }));
                if (item.Point == null)
                    validationErrorResults.Add(new ValidationResult("A Point is required", new[] { "LocationPoint" }));
                else
                {
                    if (item.Point.Lat < -90)
                        validationErrorResults.Add(new ValidationResult("A Latitude >= -90 is required", new[] { "PointLat" }));
                    else if (item.Point.Lat > 90)
                        validationErrorResults.Add(new ValidationResult("A Latitude <= 90 is required", new[] { "PointLat" }));

                    if (item.Point.Lon < -180)
                        validationErrorResults.Add(new ValidationResult("A Longitude >= -180 is required", new[] { "PointLon" }));
                    else if (item.Point.Lon > 180)
                        validationErrorResults.Add(new ValidationResult("A Longitude <= 180 is required", new[] { "PointLon" }));
                }
            }
            else
                validationErrorResults.Add(new ValidationResult("A non-null object is required.", new[] { "Spot" }));

            return validationErrorResults.Count == 0;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
