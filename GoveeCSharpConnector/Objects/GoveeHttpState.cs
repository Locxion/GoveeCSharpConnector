using System.Text.Json.Serialization;
using GoveeCSharpConnector.Objects.Misc;

namespace GoveeCSharpConnector.Objects;

public class GoveeHttpState
{
    [JsonPropertyName("requestId")]
    public string RequestId { get; set; }
    [JsonPropertyName("msg")] 
    public string Msg { get; set; }
    [JsonPropertyName("code")] 
    public long Code { get; set; }
    [JsonPropertyName("payload")] 
    public Payload Payload { get; set; }
}