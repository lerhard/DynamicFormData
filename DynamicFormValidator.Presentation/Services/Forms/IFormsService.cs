using DynamicFormValidator.Presentation.Models.DTOs;
using DynamicFormValidator.Presentation.Models.Entities.Forms;
using FluentValidation.Results;

namespace DynamicFormValidator.Presentation.Services.Forms;

public interface IFormsService
{
   Task<Form> GetForm(int id);
   Task<ValidationResult> ValidateForm(FormDto formDto);
   Task SaveForm(FormDto form);
}