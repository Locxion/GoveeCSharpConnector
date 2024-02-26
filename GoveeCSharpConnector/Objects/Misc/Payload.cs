using System.Text.Json.Serialization;

namespace GoveeCSharpConnector.Objects.Misc;

public class Payload
{
    [JsonPropertyName("sku")]
    public string Model { get; set; }
    [JsonPropertyName("device")]
    public string Device { get; set; }
    [JsonPropertyName("capabilities")]
    public List<Capability> Capabilities { get; set; }
}