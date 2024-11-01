using System.Text.Json.Serialization;

namespace DynamicFormValidator.Presentation.Models.DTOs;

public class FormRequest
{
    [JsonPropertyName("form_id")]
    public int FormId { get; init; }
    
    [JsonPropertyName("form_data")]
    public Dictionary<string,FormRequestInfo> FormData { get; init; }
}

public class FormRequestInfo
{
    [JsonPropertyName("key_id")]
    public int KeyId { get; init; }
    [JsonPropertyName("key_type")]
    public int KeyType { get; init; }
    
    [JsonPropertyName("key_value")]
    public string KeyValue { get; init; }
}
