using System.Text.RegularExpressions;
using DynamicFormValidator.Presentation.Models.DTOs;
using DynamicFormValidator.Presentation.Models.Entities.Forms;
using DynamicFormValidator.Presentation.Models.Enums;
using FluentValidation;

namespace DynamicFormValidator.Presentation.Models.Validators;

public class FormValidator : AbstractValidator<FormDto>
{
    private readonly FormFieldsValidatorInfo _fieldsInfo;

    public FormValidator(Form form, ValidationContext<FormDto> originalContext = null)
    {
        _fieldsInfo = new FormFieldsValidatorInfo(form);

        foreach (var key in _fieldsInfo.RequiredValidation.Keys)
        {
            RuleFor(x => x.Fields.ContainsKey(key)).Equal(true)
                .WithMessage(_fieldsInfo.RequiredValidation[key].ErrorMessage)
                .WithName(_fieldsInfo.Name[key]);
        }


        RuleForEach(x => x.Fields).Custom((field, context) =>
        {
            if (_fieldsInfo.Validation.TryGetValue(field.Key, out var validations))
            {
                var validationContext = originalContext ?? context;
                ApplyValidation(field, validations, validationContext);
            }
        });

        RuleForEach(x => x.SubForm).Custom((subform, context) =>
        {
            if (_fieldsInfo.Subforms.TryGetValue(subform.Id, out var subFormInfo))
            {
                RuleFor(subform => subform).SetValidator(new FormValidator(subFormInfo, originalContext ?? context));
            }
        });
    }

    private void ApplyValidation(KeyValuePair<int, string> field
        , List<FormValidationInfo> validations
        , ValidationContext<FormDto> context)
    {
        foreach (var validation in validations)
        {
            IComparable value = null;
            IComparable valueToCompare = null;

            switch ((ValidationType)validation.ValidationType)
            {
                case ValidationType.REQUIRED:
                    if (!string.IsNullOrWhiteSpace(field.Value))
                    {
                        break;
                    }

                    context.AddFailure(_fieldsInfo.Name[field.Key], validation.ErrorMessage);
                    break;

                case ValidationType.LESS_THAN:
                    value = GetComparableValue(field.Value, _fieldsInfo.Type[field.Key]);
                    valueToCompare = GetComparableValue(validation.ValidationValues[0], _fieldsInfo.Type[field.Key]);
                    if (value.CompareTo(valueToCompare) < 0)
                    {
                        break;
                    }

                    context.AddFailure(_fieldsInfo.Name[field.Key], validation.ErrorMessage);
                    break;

                case ValidationType.LESS_THAN_OR_EQUALS:

                    value = GetComparableValue(field.Value, _fieldsInfo.Type[field.Key]);
                    valueToCompare = GetComparableValue(validation.ValidationValues[0], _fieldsInfo.Type[field.Key]);

                    if (value.CompareTo(valueToCompare) is 0 or < 0)
                    {
                        break;
                    }

                    context.AddFailure(_fieldsInfo.Name[field.Key], validation.ErrorMessage);
                    break;

                case ValidationType.NOT_EQUALS:
                    value = GetComparableValue(field.Value, _fieldsInfo.Type[field.Key]);
                    valueToCompare = GetComparableValue(validation.ValidationValues[0], _fieldsInfo.Type[field.Key]);
                    if (value.CompareTo(valueToCompare) != 0)
                    {
                        break;
                    }

                    context.AddFailure(_fieldsInfo.Name[field.Key], validation.ErrorMessage);
                    break;
                case ValidationType.GREATER_THAN:
                    value = GetComparableValue(field.Value, _fieldsInfo.Type[field.Key]);
                    valueToCompare = GetComparableValue(validation.ValidationValues[0], _fieldsInfo.Type[field.Key]);
                    if (value.CompareTo(valueToCompare) > 0)
                    {
                        break;
                    }

                    context.AddFailure(_fieldsInfo.Name[field.Key], validation.ErrorMessage);
                    break;
                case ValidationType.GREATER_THAN_OR_EQUALS:
                    value = GetComparableValue(field.Value, _fieldsInfo.Type[field.Key]);
                    valueToCompare = GetComparableValue(validation.ValidationValues[0], _fieldsInfo.Type[field.Key]);
                    if (value.CompareTo(valueToCompare) is 0 or > 0)
                    {
                        break;
                    }

                    context.AddFailure(_fieldsInfo.Name[field.Key], validation.ErrorMessage);
                    break;
                case ValidationType.AGE:
                    
                    DateTime date = DateTime.Parse(field.Value);
                    DateTime currentDate = DateTime.Now;
                    int age = (currentDate - date).Days / 365;
                    int ageToCompare = int.Parse(validation.ValidationValues[0]);

                    if (age < ageToCompare)
                    {
                        context.AddFailure(_fieldsInfo.Name[field.Key], validation.ErrorMessage);
                    }

                    break;
                case ValidationType.REGEX:
                    if (Regex.IsMatch(field.Value, validation.ValidationValues[0]))
                    {
                        break;
                    }

                    context.AddFailure(_fieldsInfo.Name[field.Key], validation.ErrorMessage);
                    break;
            }
        }
    }

    private IComparable GetComparableValue(string value, DataType type)
    {
        switch (type)
        {
            case DataType.INT:
                return int.Parse(value);
            case DataType.DATE:
                return DateTime.Parse(value).Date;
            case DataType.UUID:
                return Guid.Parse(value);
            case DataType.FLOAT:
                return float.Parse(value);
            case DataType.DOUBLE:
                return double.Parse(value);
            case DataType.DECIMAL:
                return decimal.Parse(value);
            case DataType.BOOLEAN:
                return bool.Parse(value);
            case DataType.DATETIME:
                return DateTime.Parse(value);
            case DataType.STRING:
            default:
                return value;
        }
    }
}