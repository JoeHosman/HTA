using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HTA.Adventures.Models.Types;

namespace HTA.Adventures.Data.ModelValidation
{
    public sealed class AdventureSpotValidator
    {
        private readonly GenericValidator<Spot> _spotValidator;
        private readonly GenericValidator<GeoPoint> _locationValidator;
        private readonly GeoLocationValidator _geoLocationValidation;

        public AdventureSpotValidator()
        {
            _spotValidator = new GenericValidator<Spot>();
            _locationValidator = new GenericValidator<GeoPoint>();
            _geoLocationValidation = new GeoLocationValidator();
        }

        public IList<ValidationResult> Validate(Spot entity)
        {
            var results = new List<ValidationResult>();
            var spotValidationResults = _spotValidator.Validate(entity);
            results.AddRange(spotValidationResults);

            if (entity != null && entity.Point != null)
            {
                var locValidationResults = _locationValidator.Validate(entity.Point);
                results.AddRange(locValidationResults);

                results.AddRange(_geoLocationValidation.Validate(entity.Point));
            }

            return results;
        }
    }
}