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

                if (entity.Location != null)
                {
                    var regionSpotValidationResults = _adventureSpotValidator.Validate(entity.Location);
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
}