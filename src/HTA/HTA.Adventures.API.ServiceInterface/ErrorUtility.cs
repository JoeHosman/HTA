using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using ServiceStack.ServiceInterface.ServiceModel;

namespace HTA.Adventures.API.ServiceInterface
{
    class ErrorUtility
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
                ResponseError error = new ResponseError
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
    }
}
