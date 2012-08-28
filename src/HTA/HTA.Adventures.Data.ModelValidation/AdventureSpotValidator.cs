using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HTA.Adventures.Models.Types;

namespace HTA.Adventures.Data.ModelValidation
{
    public sealed class AdventureReviewValidator
    {
        private readonly GenericValidator<AdventureReview> _reviewValidator;
        private readonly GenericValidator<AdventureType> _typeValidator;
        private readonly AdventureSpotValidator _adventureSpotValidator;

        public AdventureReviewValidator()
        {
            _reviewValidator = new GenericValidator<AdventureReview>();
            _typeValidator = new GenericValidator<AdventureType>();
            _adventureSpotValidator = new AdventureSpotValidator();
        }

        public IList<ValidationResult> Validate(AdventureReview entity)
        {
            var results = new List<ValidationResult>();
            var spotValidationResults = _reviewValidator.Validate(entity);
            results.AddRange(spotValidationResults);

            if (entity != null)
            {
                if (entity.AdventureType != null)
                {
                    var typeValidationResults = _typeValidator.Validate(entity.AdventureType);
                    results.AddRange(typeValidationResults);
                }
                else
                {
                    results.Add(new ValidationResult("Null AdventureType"));
                }

                if (entity.AdventureLocation != null)
                {
                    var regionSpotValidationResults = _adventureSpotValidator.Validate(entity.AdventureLocation);
                    results.AddRange(regionSpotValidationResults);
                }
                else
                {
                    results.Add(new ValidationResult("Null AdventureLocation"));
                }
            }

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

            if (entity != null && entity.LocationPoint != null)
            {
                var locValidationResults = _locationValidator.Validate(entity.LocationPoint);
                results.AddRange(locValidationResults);

                results.AddRange(_geoLocationValidation.Validate(entity.LocationPoint));
            }

            return results;
        }
    }
}