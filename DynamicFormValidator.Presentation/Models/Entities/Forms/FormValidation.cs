using System.ComponentModel.DataAnnotations.Schema;

namespace DynamicFormValidator.Presentation.Models.Entities.Forms;

public class FormValidation
{
    public int FormId { get; set; }
    public int FieldId { get; set; }
    public List<ValidationInfo> Validations { get; set; }
}