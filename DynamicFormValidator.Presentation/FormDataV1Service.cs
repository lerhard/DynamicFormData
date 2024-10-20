namespace DynamicFormValidator.Presentation;

public static class FormDataV1Service
{
    public static Dictionary<int,FormDataValidationInfo[]> GetValidationInfo(int formId)
    {
        if (formId == 1)
        {
            return Form1();
        }

        return default;
    }

    private static Dictionary<int, FormDataValidationInfo[]> Form1()
    {
        var dic = new Dictionary<int, FormDataValidationInfo[]>();
        dic.Add(1, new FormDataValidationInfo[]
        {
            new FormDataValidationInfo
            {
                FormId = 1,
                KeyId = 1,
                KeyName = "Name",
                KeyType = (int)FormDataType.STRING,
                Validations = new List<ValidationInfo>
                {
                    new ValidationInfo
                    {
                        ValidationType = (int)FormDataValidationType.REQUIRED,
                        ErrorMessage = "Name is required"
                    }
                }
            },
        });

        dic.Add(2, new FormDataValidationInfo[]
            {
                new FormDataValidationInfo()
                {
                    FormId = 1,
                    KeyId = 2,
                    KeyName = "Age",
                    KeyType = (int)FormDataType.INT,
                    Validations = new List<ValidationInfo>
                    {
                        new ValidationInfo
                        {
                            ValidationType = (int)FormDataValidationType.REQUIRED,
                            ErrorMessage = "Age is required"
                        },
                        new ValidationInfo
                        {
                            ValidationType = (int)FormDataValidationType.GREATER_THAN_OR_EQUALS,
                            ValidationValues = new string[] { "18" },
                            ErrorMessage = "Age must be greater than or equals to 18"
                        }
                    }
                }
            }
        );

        return dic;
    }
}