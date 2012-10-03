using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HTA.Adventures.Models.Types;

namespace HTA.Adventures.BusinessLogic
{
    public class AdventureTypeBusiness : IDisposable
    {
        public bool Validate(AdventureType item, IList<ValidationResult> validationErrorResults)
        {
            if (null == validationErrorResults)
                validationErrorResults = new List<ValidationResult>();

            if (null == item)
            {
                validationErrorResults.Add(new ValidationResult("A non-null object is required.", new[] { "AdventureType" }));
            }
            else
            {
                if (string.IsNullOrEmpty(item.Name))
                    validationErrorResults.Add(new ValidationResult("A Name is required", new[] { "TypeName" }));
                if (string.IsNullOrEmpty(item.Description))
                    validationErrorResults.Add(new ValidationResult("A Description is required", new[] { "TypeDescription" }));
                if (null == item.DataCardTemplates)
                {
                    validationErrorResults.Add(new ValidationResult("a non-null object is required.", new[] { "TypeDataCardTemplateList" }));
                }
                else
                {
                    using (var typeTemplateBusiness = new AdventureTypeTemplateBusiness())
                    {
                        foreach (AdventureTypeTemplate adventureTypeTemplate in item.DataCardTemplates)
                        {
                            bool isValid = typeTemplateBusiness.Validate(adventureTypeTemplate, validationErrorResults);
                        }

                    }
                }
            }

            return validationErrorResults.Count == 0;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}