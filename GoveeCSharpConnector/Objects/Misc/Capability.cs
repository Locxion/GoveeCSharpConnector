using System.Text.Json.Serialization;

namespace GoveeCSharpConnector.Objects.Misc;

public class Capability
{
    [JsonPropertyName("type")]
    public string Type { get; set; }
    [JsonPropertyName("instance")]
    public string Instance { get; set; }
    [JsonPropertyName("state")]
    public State State { get; set; }
}