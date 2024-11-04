using DynamicFormValidator.Presentation.Models.Enums;

namespace DynamicFormValidator.Presentation.Models.Entities.Forms;

public class FormField
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Label { get; set; }
    public int type { get; set; }
    public bool Hidden { get; set; }
}