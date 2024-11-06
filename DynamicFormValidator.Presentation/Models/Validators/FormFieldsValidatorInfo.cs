using DynamicFormValidator.Presentation.Models.Entities.Forms;
using DynamicFormValidator.Presentation.Models.Enums;

namespace DynamicFormValidator.Presentation.Models.Validators;

public class FormFieldsValidatorInfo
{
    private FormFieldsValidatorInfo()
    {
    }

    public FormFieldsValidatorInfo(Form form)
    {
        Name = form.Fields.ToDictionary(f => f.Id, f => f.Name);
        Type = form.Fields.ToDictionary(f => f.Id, f => (DataType)f.type);
        Validation = form.ValidationInfo
            .Where(v => v.ValidationType != (int)ValidationType.REQUIRED)
            .GroupBy(v => v.FieldId, v => v)
            .ToDictionary(g => g.Key, g =>
                g.OrderBy(x => x.ValidationType).ToList());

        RequiredValidation = form
            .ValidationInfo.Where(v => v.ValidationType == (int)ValidationType.REQUIRED)
            .ToDictionary(x => x.FieldId, x => x);
        
        IgnoreInsert = form.ValidationInfo.ToDictionary(x => x.Id, x => x.IgnoreOnInsert);
        IgnoreUpdate = form.ValidationInfo.ToDictionary(x => x.Id, x => x.IgnoreOnUpdate);
        
        
        
        if (form.SubForms != null)
        {
            Subforms = form.SubForms.ToDictionary(f => f.Id, f => f);
        }
        else
        {
            Subforms = new Dictionary<int, Form>();
        }
    }

    public Dictionary<int, List<FormValidationInfo>> Validation { get; init; }
    public Dictionary<int,FormValidationInfo> RequiredValidation { get; init; }
    public Dictionary<int, string> Name { get; init; }
    public Dictionary<int, DataType> Type { get; init; }
    public Dictionary<int, Form> Subforms { get; init; }
    public Dictionary<int, bool> IgnoreInsert { get; init; }
    public Dictionary<int, bool> IgnoreUpdate { get; init; }
}