using DynamicFormValidator.Presentation.Models;
using DynamicFormValidator.Presentation.Models.Enums;

namespace DynamicFormValidator.Presentation.Services;

public static class FormService
{
    public static Dictionary<int,RequestValidationInfo[]> GetValidationInfo(int formId)
    {
        return formId == 1 ? Form1() : default;
    }

    private static Dictionary<int, RequestValidationInfo[]> Form1()
    {
        var dic = new Dictionary<int, RequestValidationInfo[]>();
        dic.Add(1, new RequestValidationInfo[]
        {
            new RequestValidationInfo
            {
                FormId = 1,
                KeyId = 1,
                KeyName = "Name",
                KeyType = (int)DataType.STRING,
                Validations = new List<ValidationInfo>
                {
                    new ValidationInfo
                    {
                        ValidationType = (int)ValidationType.REQUIRED,
                        ErrorMessage = "Name is required"
                    }
                }
            },
        });

        dic.Add(2, new RequestValidationInfo[]
            {
                new RequestValidationInfo()
                {
                    FormId = 1,
                    KeyId = 2,
                    KeyName = "Age",
                    KeyType = (int)DataType.INT,
                    Validations = new List<ValidationInfo>
                    {
                        new ValidationInfo
                        {
                            ValidationType = (int)ValidationType.REQUIRED,
                            ErrorMessage = "Age is required"
                        },
                        new ValidationInfo
                        {
                            ValidationType = (int)ValidationType.GREATER_THAN_OR_EQUALS,
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