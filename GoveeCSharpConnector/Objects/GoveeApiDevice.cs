using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GoveeCSharpConnector.Objects;

public class GoveeApiDevice
{
    [JsonPropertyName("device")]
    public string DeviceId { get; set; }
    public string Model { get; set; }
    public string DeviceName { get; set; }
    public bool Controllable { get; set; }
    public bool Retrievable { get; set; }
    [JsonPropertyName("supportCmds")]
    public List<string> SupportedCommands { get; set; }
    public Properties Properties { get; set; }
}