using System.Text.Json.Serialization;
using GoveeCSharpConnector.Objects.Misc;

namespace GoveeCSharpConnector.Objects;

public class GoveeHttpDevice
{
    [JsonPropertyName("sku")]
    public string Model { get; set; }
    [JsonPropertyName("device")]
    public string Device { get; set; }
    [JsonPropertyName("capabilities")]
    public List<Capability> Capabilities { get; set; }
}