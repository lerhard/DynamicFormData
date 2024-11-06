using DynamicFormValidator.Presentation.Models.DTOs;
using DynamicFormValidator.Presentation.Models.Entities.Forms;
using DynamicFormValidator.Presentation.Models.Enums;
using FluentValidation.Results;

namespace DynamicFormValidator.Presentation.Services.Forms;

public interface IFormsService
{
   Task<Form> GetForm(int id);
   Task InsertForm(FormDto formDto);
   Task UpdateForm(FormDto formDto);
   
   Task<object> SelectForm(string entityId, int formId);
   Task<ValidationResult> ValidateForm(FormDto formDto, FormRequestOperation operation = FormRequestOperation.INSERT);
   Task<ValidationResult> ValidateEntityId(string entityId);
   Task DeleteForm(int formId, string entityId);
}