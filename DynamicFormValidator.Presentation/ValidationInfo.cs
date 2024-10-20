namespace DynamicFormValidator.Presentation;

public class ValidationInfo
{
    public int ValidationType { get; set; }
    public string[] ValidationValues { get; set; }
    public string ErrorMessage { get; set; }
}