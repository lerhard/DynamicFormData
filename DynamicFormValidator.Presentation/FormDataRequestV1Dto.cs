using System.Text.Json.Serialization;

namespace DynamicFormValidator.Presentation;

public class FormDataRequestV1Dto
{
    [JsonPropertyName("form_id")]
    public int FormId { get; init; }
    
    [JsonPropertyName("form_data")]
    public Dictionary<string,FormDataRequestInfo> FormData { get; init; }
}

public class FormDataRequestInfo
{
    [JsonPropertyName("key_id")]
    public int KeyId { get; init; }
    [JsonPropertyName("key_type")]
    public int KeyType { get; init; }
    
    [JsonPropertyName("key_value")]
    public string KeyValue { get; init; }
}
