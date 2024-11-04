using DynamicFormValidator.Presentation.Models.Entities.Forms;

namespace DynamicFormValidator.Presentation.Services.Forms;

public interface IFormsService
{
   Task<Form> GetForm(int id);
}