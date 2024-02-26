using System.Text.Json.Serialization;

namespace GoveeCSharpConnector.Objects.Misc;

public class State
{
    [JsonPropertyName("value")]
    public object Value { get; set; }
}