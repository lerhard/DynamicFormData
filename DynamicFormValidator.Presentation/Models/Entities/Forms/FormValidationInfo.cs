using Newtonsoft.Json;

namespace DynamicFormValidator.Presentation.Models.Entities.Forms;

public class FormValidationInfo
{
    [JsonProperty("id")]
    public int Id { get; set; }
    
    [JsonProperty("field_id")]
    public int FieldId { get; set; }
    
    [JsonProperty("validation_type")]
    public int ValidationType { get; set; }
    
    [JsonProperty("validation_values")]
    public string[] ValidationValues { get; set; }
    
    [JsonProperty("error_message")]
    public string ErrorMessage { get; set; } 
    
    [JsonProperty("ignore_on_update")]
    public bool IgnoreOnUpdate { get; set; }
    
    [JsonProperty("ignore_on_insert")]
    public bool IgnoreOnInsert { get; set; }
    
}