using System.Text.Json.Serialization;
using GoveeCSharpConnector.Objects.Misc;

namespace GoveeCSharpConnector.Objects;

public class GoveeScene
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("value")]
    public SceneValue SceneValue { get; set; }
}