namespace DynamicFormValidator.Presentation.Models;

public class RequestValidationInfo
{
    public int FormId { get; init; }
    public int KeyId { get; init; }
    public string KeyName { get; init; }
    public int KeyType { get; init; }
    public List<ValidationInfo> Validations { get; init; }
}

