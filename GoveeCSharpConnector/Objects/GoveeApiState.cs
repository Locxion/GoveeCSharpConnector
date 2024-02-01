using System.Text.Json.Serialization;

namespace GoveeCSharpConnector.Objects;

public class GoveeApiState
{
    [JsonPropertyName("device")]
    public string DeviceId { get; set; }

    public string Model { get; set; }
    
    public string Name { get; set; }
    
    [JsonIgnore]
    public Properties Properties { get; set; }
}