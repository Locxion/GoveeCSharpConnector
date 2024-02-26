using System.Text.Json.Serialization;

namespace GoveeCSharpConnector.Objects.Misc;

public class SceneValue
{
    [JsonPropertyName("paramId")]
    public long ParamId { get; set; }

    [JsonPropertyName("id")]
    public long Id { get; set; }
}