using DynamicFormValidator.Presentation.Models.DTOs;
using DynamicFormValidator.Presentation.Models.Enums;
using DynamicFormValidator.Presentation.Services;
using FluentValidation.Results;

namespace DynamicFormValidator.Presentation.Models.Validators;

public class RequestValidatorService
{
    public static ValidationResult Validate(FormRequest formRequest)
    {
        Dictionary<int, RequestValidationInfo[]> formValidationInfo =
            FormService.GetValidationInfo(formRequest.FormId);

        ValidationResult validationResult = new ValidationResult();

        foreach (var key in formRequest.FormData.Keys)
        {
            if (!formValidationInfo.ContainsKey(formRequest.FormData[key].KeyId))
            {
                throw new NullReferenceException("Key not found in validation info. FormId: "
                                                 + formRequest.FormId + " KeyId: " + formRequest.FormData[key].KeyId);
            }

            var validationInfo = formValidationInfo[formRequest.FormData[key].KeyId];


            foreach (var valInfo in validationInfo)
            {
                foreach (var validation in valInfo.Validations)
                {
                    switch ((ValidationType)validation.ValidationType)
                    {
                        case ValidationType.REQUIRED:
                            if (string.IsNullOrWhiteSpace(formRequest.FormData[key].KeyValue))
                            {
                                validationResult.Errors.Add(new ValidationFailure(key, validation.ErrorMessage));
                            }

                            break;

                        case ValidationType.EQUALS:

                            if (string.IsNullOrWhiteSpace(formRequest.FormData[key].KeyValue))
                            {
                                validationResult.Errors.Add(new ValidationFailure(key, validation.ErrorMessage));
                            }

                            if (valInfo.KeyType == (int)DataType.STRING)
                            {
                                foreach (var validationValue in validation.ValidationValues)
                                {
                                    if (!string.Equals(validationValue, formRequest.FormData[key].KeyValue))
                                    {
                                        validationResult.Errors.Add(new ValidationFailure(key,
                                            validation.ErrorMessage));
                                    }
                                }
                            }

                            if (valInfo.KeyType == (int)DataType.INT)
                            {
                                int formValue = int.Parse(formRequest.FormData[key].KeyValue);
                                foreach (var validationValue in validation.ValidationValues)
                                {
                                    if (formValue != int.Parse(validationValue))
                                    {
                                        validationResult.Errors.Add(new ValidationFailure(key,
                                            validation.ErrorMessage));
                                    }
                                }
                            }

                            if (valInfo.KeyType == (int)DataType.FLOAT)
                            {
                                var formValue = float.Parse(formRequest.FormData[key].KeyValue);
                                foreach (var validationValue in validation.ValidationValues)
                                {
                                    if (formValue != float.Parse(validationValue))
                                    {
                                        validationResult.Errors.Add(new ValidationFailure(key,
                                            validation.ErrorMessage));
                                    }
                                }
                            }

                            if (valInfo.KeyType == (int)DataType.DOUBLE)
                            {
                                var formValue = double.Parse(formRequest.FormData[key].KeyValue);
                                foreach (var validationValue in validation.ValidationValues)
                                {
                                    if (formValue != double.Parse(validationValue))
                                    {
                                        validationResult.Errors.Add(new ValidationFailure(key,
                                            validation.ErrorMessage));
                                    }
                                }
                            }

                            if (valInfo.KeyType == (int)DataType.DECIMAL)
                            {
                                var formValue = decimal.Parse(formRequest.FormData[key].KeyValue);
                                foreach (var validationValue in validation.ValidationValues)
                                {
                                    if (formValue != decimal.Parse(validationValue))
                                    {
                                        validationResult.Errors.Add(new ValidationFailure(key,
                                            validation.ErrorMessage));
                                    }
                                }
                            }

                            if (valInfo.KeyType == (int)DataType.DATE)
                            {
                                var formValue = DateTime.Parse(formRequest.FormData[key].KeyValue);
                                foreach (var validationValue in validation.ValidationValues)
                                {
                                    if (formValue.Date != DateTime.Parse(validationValue).Date)
                                    {
                                        validationResult.Errors.Add(new ValidationFailure(key,
                                            validation.ErrorMessage));
                                    }
                                }
                            }

                            break;

                        case ValidationType.GREATER_THAN_OR_EQUALS:

                            if (string.IsNullOrWhiteSpace(formRequest.FormData[key].KeyValue))
                            {
                                validationResult.Errors.Add(new ValidationFailure(key, validation.ErrorMessage));
                            }


                            if (valInfo.KeyType == (int)DataType.INT)
                            {
                                int formValue = int.Parse(formRequest.FormData[key].KeyValue);
                                foreach (var validationValue in validation.ValidationValues)
                                {
                                    if ( formValue < int.Parse(validationValue))
                                    {
                                        validationResult.Errors.Add(new ValidationFailure(key,
                                            validation.ErrorMessage));
                                    }
                                }
                            }

                            if (valInfo.KeyType == (int)DataType.FLOAT)
                            {
                                var formValue = float.Parse(formRequest.FormData[key].KeyValue);
                                foreach (var validationValue in validation.ValidationValues)
                                {
                                    if (formValue < float.Parse(validationValue))
                                    {
                                        validationResult.Errors.Add(new ValidationFailure(key,
                                            validation.ErrorMessage));
                                    }
                                }
                            }

                            if (valInfo.KeyType == (int)DataType.DOUBLE)
                            {
                                var formValue = double.Parse(formRequest.FormData[key].KeyValue);
                                foreach (var validationValue in validation.ValidationValues)
                                {
                                    if (formValue < double.Parse(validationValue))
                                    {
                                        validationResult.Errors.Add(new ValidationFailure(key,
                                            validation.ErrorMessage));
                                    }
                                }
                            }

                            if (valInfo.KeyType == (int)DataType.DECIMAL)
                            {
                                var formValue = decimal.Parse(formRequest.FormData[key].KeyValue);
                                foreach (var validationValue in validation.ValidationValues)
                                {
                                    if (formValue < decimal.Parse(validationValue))
                                    {
                                        validationResult.Errors.Add(new ValidationFailure(key,
                                            validation.ErrorMessage));
                                    }
                                }
                            }

                            if (valInfo.KeyType == (int)DataType.DATE)
                            {
                                var formValue = DateTime.Parse(formRequest.FormData[key].KeyValue);
                                foreach (var validationValue in validation.ValidationValues)
                                {
                                    if (formValue.Date < DateTime.Parse(validationValue).Date)
                                    {
                                        validationResult.Errors.Add(new ValidationFailure(key,
                                            validation.ErrorMessage));
                                    }
                                }
                            }

                            break;

                        default:
                            break;
                    }
                }
            }
        }

        return validationResult;
    }
}