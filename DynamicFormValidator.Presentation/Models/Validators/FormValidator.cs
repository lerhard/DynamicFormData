using DynamicFormValidator.Presentation.Models.DTOs;
using DynamicFormValidator.Presentation.Models.Entities.Forms;
using FluentValidation;

namespace DynamicFormValidator.Presentation.Models.Validators;

public class FormValidator:AbstractValidator<FormDto>
{
    public FormValidator(Form form, FormDto dto)
    {
    }
}