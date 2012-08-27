using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace HTA.Adventures.Data.ModelValidation
{


    /// <summary>
    /// http://www.codeproject.com/Articles/185943/Validation-in-NET-Framework-4
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class GenericValidator<T> where T : class
    {
        public const int ValidResultCount = 0;
        public IList<ValidationResult> Validate(T entity)
        {
            var results = new List<ValidationResult>();
            if (entity == null)
            {
                results.Add(new ValidationResult("Entrity was null"));
                return results;
            }
            var context = new ValidationContext(entity, null, null);
            Validator.TryValidateObject(entity, context, results);
            return results;
        }
    }
}
