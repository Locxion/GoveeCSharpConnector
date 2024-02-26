using System.Text.Json.Serialization;

namespace GoveeCSharpConnector.Objects.Misc;

public class Field
{
    [JsonPropertyName("fieldName")]
    public string FieldName { get; set; }
    [JsonPropertyName("dataType")]
    public string DataType { get; set; }
    [JsonPropertyName("options")]
    public List<Option> Options { get; set; }
    [JsonPropertyName("required")]
    public bool Required { get; set; }
    [JsonPropertyName("range")]
    public Range Range { get; set; }
    [JsonPropertyName("unit")]
    public string Unit { get; set; }
    [JsonPropertyName("reauired")]
    public bool? Reauired { get; set; }
}