namespace DynamicFormValidator.Presentation;

public class FormDataValidationInfo
{
    public int FormId { get; init; }
    public int KeyId { get; init; }
    public string KeyName { get; init; }
    public int KeyType { get; init; }
    public List<ValidationInfo> Validations { get; init; }
}

