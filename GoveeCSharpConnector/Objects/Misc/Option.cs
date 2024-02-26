using System.Text.Json.Serialization;

namespace GoveeCSharpConnector.Objects.Misc;

public class Option
{
    [JsonPropertyName("name")]
    public string Name { get; set; }
    [JsonPropertyName("value")]
    public int Value { get; set; }
}