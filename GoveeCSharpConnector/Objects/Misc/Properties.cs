using System.Text.Json.Serialization;
using GoveeCSharpConnector.Enums;

namespace GoveeCSharpConnector.Objects.Misc;

public class Properties
{
    [JsonPropertyName("online")]
    public bool Online { get; set; }
    [JsonPropertyName("powerState")]
    public PowerState PowerState { get; set; }
    [JsonPropertyName("brightness")]
    public int Brightness { get; set; }
    [JsonPropertyName("colorTemp")]
    public int ColorTemp { get; set; }
    [JsonPropertyName("color")]
    public RgbColor Color { get; set; }
}