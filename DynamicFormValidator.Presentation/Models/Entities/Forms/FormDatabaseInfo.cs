using DynamicFormValidator.Presentation.Models.Enums;
using Newtonsoft.Json;

namespace DynamicFormValidator.Presentation.Models.Entities.Forms;

public class FormDatabaseInfo
{
    [JsonProperty("id")]
    public int Id { get; set; }
    
    [JsonProperty("field_id")]
    public int FieldId { get; set; }
    
    [JsonProperty("table")]
    public string Table { get; set; }
    
    [JsonProperty("column_name")]
    public string ColumnName { get; set; }
    
    [JsonProperty("type")]
    public int Type { get; set; }
    
    [JsonProperty("is_primary_key")]
    public bool IsPrimaryKey { get; set; }
}