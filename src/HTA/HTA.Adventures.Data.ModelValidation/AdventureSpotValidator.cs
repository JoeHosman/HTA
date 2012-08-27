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
    public sealed class AdventureSpotValidator
    {
        private readonly GenericValidator<AdventureSpot> _spotValidator;
        private readonly GenericValidator<LocationPoint> _locationValidator;
        private readonly GeoLocationValidator _geoLocationValidation;

        public AdventureSpotValidator()
        {
            _spotValidator = new GenericValidator<AdventureSpot>();
            _locationValidator = new GenericValidator<LocationPoint>();
            _geoLocationValidation = new GeoLocationValidator();
        }

        public IList<ValidationResult> Validate(AdventureSpot entity)
        {
            var results = new List<ValidationResult>();
            var spotValidationResults = _spotValidator.Validate(entity);
            results.AddRange(spotValidationResults);

            if (entity!= null &&  entity.LocationPoint != null)
            {
                var locValidationResults = _locationValidator.Validate(entity.LocationPoint);
                results.AddRange(locValidationResults);

                results.AddRange(_geoLocationValidation.Validate(entity.LocationPoint));
            }

            return results;
        }
    }
}