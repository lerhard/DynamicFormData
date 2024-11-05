using System.Data;
using DynamicFormValidator.Presentation.Models.DTOs;
using DynamicFormValidator.Presentation.Models.Entities.Forms;

namespace DynamicFormValidator.Presentation.Models.Interpreter;

public interface IFormDataQueryInterpreter
{
   Task<bool> InsertFormData(FormDto formDto,Form form);
   Task<bool> DeleteFormData(string entityId, Form form);
}