using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using ServiceStack.ServiceInterface.ServiceModel;

namespace HTA.Adventures.API.ServiceInterface
{
    public class ErrorUtility
    {
        public static void TransformResponseErrors(IList<ResponseError> responseErrors, IList<ValidationResult> validationErrorResults)
        {
            if (responseErrors == null)
            {
                responseErrors = new List<ResponseError>(validationErrorResults.Count);
            }
            foreach (var validationResult in validationErrorResults)
            {
                var member = validationResult.MemberNames.GetEnumerator();
                member.MoveNext();
                var error = new ResponseError
                                          {
                                              Message = validationResult.ErrorMessage,
                                              ErrorCode = "FailedValidation"
                                          };

                try
                {
                    error.FieldName = member.Current;
                }
                catch (Exception)
                {
                }

                responseErrors.Add(error);
            }
        }

        public static void TransformResponseErrors(ModelStateDictionary responseErrors, IList<ValidationResult> validationErrorResults)
        {
            if (responseErrors == null)
            {
                responseErrors = new ModelStateDictionary();
            }

            foreach (var validationResult in validationErrorResults)
            {
                var member = validationResult.MemberNames.GetEnumerator();
                member.MoveNext();

                try
                {
                    responseErrors.AddModelError(member.Current, validationResult.ErrorMessage);
                }
                catch (Exception)
                {
                    responseErrors.AddModelError("null_name", validationResult.ErrorMessage);
                }
            }
        }
    }
}
