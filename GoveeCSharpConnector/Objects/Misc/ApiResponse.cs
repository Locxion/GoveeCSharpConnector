using System.Text.Json.Serialization;

namespace GoveeCSharpConnector.Objects.Misc;

public class ApiResponse
{
    [JsonPropertyName("code")]
    public int Code { get; set; }
    [JsonPropertyName("message")]
    public string Message { get; set; }
    [JsonPropertyName("data")]
    public List<object> Data { get; set; }
}