using DynamicFormValidator.Presentation.Models.DTOs;
using DynamicFormValidator.Presentation.Models.Entities.Forms;
using FluentValidation.Results;

namespace DynamicFormValidator.Presentation.Services.Forms;

public interface IFormsService
{
   Task<Form> GetForm(int id);
   Task SaveForm(FormDto formDto);
   Task<ValidationResult> ValidateForm(FormDto formDto);
   Task<ValidationResult> ValidateEntityId(string entityId);
   Task DeleteForm(int formId, string entityId);
}