namespace DynamicFormValidator.Presentation.Models;

public class ValidationInfo
{
    public int FieldId { get; set; }
    public int ValidationType { get; set; }
    public string[] ValidationValues { get; set; }
    public string ErrorMessage { get; set; }
}