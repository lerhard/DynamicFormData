using FluentValidation.Results;

namespace DynamicFormValidator.Presentation;

public class FormDataRequestV1Validator
{
    public ValidationResult Validate(FormDataRequestV1Dto request)
    {
        Dictionary<int, FormDataValidationInfo[]> formValidationInfo =
            FormDataV1Service.GetValidationInfo(request.FormId);

        ValidationResult validationResult = new ValidationResult();

        foreach (var key in request.FormData.Keys)
        {
            if (!formValidationInfo.ContainsKey(request.FormData[key].KeyId))
            {
                throw new NullReferenceException("Key not found in validation info. FormId: "
                                                 + request.FormId + " KeyId: " + request.FormData[key].KeyId);
            }

            var validationInfo = formValidationInfo[request.FormData[key].KeyId];


            foreach (var valInfo in validationInfo)
            {
                foreach (var validation in valInfo.Validations)
                {
                    switch ((FormDataValidationType)validation.ValidationType)
                    {
                        case FormDataValidationType.REQUIRED:
                            if (string.IsNullOrWhiteSpace(request.FormData[key].KeyValue))
                            {
                                validationResult.Errors.Add(new ValidationFailure(key, validation.ErrorMessage));
                            }

                            break;

                        case FormDataValidationType.EQUALS:

                            if (string.IsNullOrWhiteSpace(request.FormData[key].KeyValue))
                            {
                                validationResult.Errors.Add(new ValidationFailure(key, validation.ErrorMessage));
                            }

                            if (valInfo.KeyType == (int)FormDataType.STRING)
                            {
                                foreach (var validationValue in validation.ValidationValues)
                                {
                                    if (!string.Equals(validationValue, request.FormData[key].KeyValue))
                                    {
                                        validationResult.Errors.Add(new ValidationFailure(key,
                                            validation.ErrorMessage));
                                    }
                                }
                            }

                            if (valInfo.KeyType == (int)FormDataType.INT)
                            {
                                int formValue = int.Parse(request.FormData[key].KeyValue);
                                foreach (var validationValue in validation.ValidationValues)
                                {
                                    if (formValue != int.Parse(validationValue))
                                    {
                                        validationResult.Errors.Add(new ValidationFailure(key,
                                            validation.ErrorMessage));
                                    }
                                }
                            }

                            if (valInfo.KeyType == (int)FormDataType.FLOAT)
                            {
                                var formValue = float.Parse(request.FormData[key].KeyValue);
                                foreach (var validationValue in validation.ValidationValues)
                                {
                                    if (formValue != float.Parse(validationValue))
                                    {
                                        validationResult.Errors.Add(new ValidationFailure(key,
                                            validation.ErrorMessage));
                                    }
                                }
                            }

                            if (valInfo.KeyType == (int)FormDataType.DOUBLE)
                            {
                                var formValue = double.Parse(request.FormData[key].KeyValue);
                                foreach (var validationValue in validation.ValidationValues)
                                {
                                    if (formValue != double.Parse(validationValue))
                                    {
                                        validationResult.Errors.Add(new ValidationFailure(key,
                                            validation.ErrorMessage));
                                    }
                                }
                            }

                            if (valInfo.KeyType == (int)FormDataType.DECIMAL)
                            {
                                var formValue = decimal.Parse(request.FormData[key].KeyValue);
                                foreach (var validationValue in validation.ValidationValues)
                                {
                                    if (formValue != decimal.Parse(validationValue))
                                    {
                                        validationResult.Errors.Add(new ValidationFailure(key,
                                            validation.ErrorMessage));
                                    }
                                }
                            }

                            if (valInfo.KeyType == (int)FormDataType.DATE)
                            {
                                var formValue = DateTime.Parse(request.FormData[key].KeyValue);
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

                        case FormDataValidationType.GREATER_THAN_OR_EQUALS:

                            if (string.IsNullOrWhiteSpace(request.FormData[key].KeyValue))
                            {
                                validationResult.Errors.Add(new ValidationFailure(key, validation.ErrorMessage));
                            }


                            if (valInfo.KeyType == (int)FormDataType.INT)
                            {
                                int formValue = int.Parse(request.FormData[key].KeyValue);
                                foreach (var validationValue in validation.ValidationValues)
                                {
                                    if ( formValue < int.Parse(validationValue))
                                    {
                                        validationResult.Errors.Add(new ValidationFailure(key,
                                            validation.ErrorMessage));
                                    }
                                }
                            }

                            if (valInfo.KeyType == (int)FormDataType.FLOAT)
                            {
                                var formValue = float.Parse(request.FormData[key].KeyValue);
                                foreach (var validationValue in validation.ValidationValues)
                                {
                                    if (formValue < float.Parse(validationValue))
                                    {
                                        validationResult.Errors.Add(new ValidationFailure(key,
                                            validation.ErrorMessage));
                                    }
                                }
                            }

                            if (valInfo.KeyType == (int)FormDataType.DOUBLE)
                            {
                                var formValue = double.Parse(request.FormData[key].KeyValue);
                                foreach (var validationValue in validation.ValidationValues)
                                {
                                    if (formValue < double.Parse(validationValue))
                                    {
                                        validationResult.Errors.Add(new ValidationFailure(key,
                                            validation.ErrorMessage));
                                    }
                                }
                            }

                            if (valInfo.KeyType == (int)FormDataType.DECIMAL)
                            {
                                var formValue = decimal.Parse(request.FormData[key].KeyValue);
                                foreach (var validationValue in validation.ValidationValues)
                                {
                                    if (formValue < decimal.Parse(validationValue))
                                    {
                                        validationResult.Errors.Add(new ValidationFailure(key,
                                            validation.ErrorMessage));
                                    }
                                }
                            }

                            if (valInfo.KeyType == (int)FormDataType.DATE)
                            {
                                var formValue = DateTime.Parse(request.FormData[key].KeyValue);
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