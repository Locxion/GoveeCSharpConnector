using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using GoveeCSharpConnector.Interfaces;
using GoveeCSharpConnector.Objects;
using GoveeCSharpConnector.Objects.Misc;

namespace GoveeCSharpConnector.Services;

public class GoveeHttpService : IGoveeHttpService
{
    private string _apiKey = string.Empty;
    private const string GoveeApiAddress = "https://openapi.api.govee.com";
    private const string GoveeDevicesEndpoint = "/router/api/v1/user/devices";
    private const string GoveeControlEndpoint = "/router/api/v1/device/control";
    private const string GoveeStateEndpoint = "/router/api/v1/device/state";
    private const string GoveeScenesEndpoint = "/router/api/v1/device/scenes";
    private readonly HttpClient _httpClient = new();
    private readonly JsonSerializerOptions? _jsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };

    public GoveeHttpService()
    {
        _httpClient.DefaultRequestHeaders.Add("Content-Type", "application/json");
    }
    /// <inheritdoc/>
    public void SetApiKey(string apiKey)
    {
        _apiKey = apiKey;
        _httpClient.DefaultRequestHeaders.Add("Govee-API-Key", _apiKey);
    }
    /// <inheritdoc/>
    public string GetApiKey()
    {
        return _apiKey;
    }
    /// <inheritdoc/>
    public void RemoveApiKey()
    {
        _apiKey = string.Empty;
        _httpClient.DefaultRequestHeaders.Remove("Govee-API-Key");
    }
    /// <inheritdoc/>
    public async Task<ServiceResponse<List<GoveeHttpDevice>>> GetDevices()
    {
        var serviceResponse = new ServiceResponse<List<GoveeHttpDevice>>();
        var response = await _httpClient.GetFromJsonAsync<ApiResponse>($"{GoveeApiAddress}{GoveeDevicesEndpoint}");
        if (response.Code != 200)
        {
            if (response.Code == 429)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "Api Limit reached! 10000/Account/Day";
                return serviceResponse;
            }
            serviceResponse.Success = false;
            serviceResponse.Message = response.Message;
            return serviceResponse;
        }
        var allDevices = response.Data.OfType<GoveeHttpDevice>().ToList();

        var devices = (from device in allDevices where device.Capabilities.Exists(x => x.Type == "devices.capabilities.on_off")
            where device.Capabilities.Exists(x => x.Type == "devices.capabilities.color_setting")
            where device.Capabilities.Exists(x => x.Type == "devices.capabilities.range")
            where device.Capabilities.Exists(x => x.Type == "devices.capabilities.dynamic_scene")
            select device).ToList();

        serviceResponse.Success = true;
        serviceResponse.Data = devices;
        serviceResponse.Message = "Request Successful";
        return serviceResponse;
    }
    /// <inheritdoc/>
    public async Task<ServiceResponse<GoveeHttpState>> GetDeviceState(string deviceId, string deviceModel)
    {
        var serviceResponse = new ServiceResponse<GoveeHttpState>();

        var jsonPayload = $@"
        {{
            ""requestId"": ""{Guid.NewGuid()}"",
            ""payload"": {{
                ""sku"": ""{deviceModel}"",
                ""device"": ""{deviceId}""
            }}
        }}";
        
        var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync($"{GoveeApiAddress}{GoveeStateEndpoint}", content);
        if (!response.IsSuccessStatusCode)
        {
            serviceResponse.Success = false;
            serviceResponse.Message = response.ReasonPhrase;
            return serviceResponse;
        }

        var state = await response.Content.ReadFromJsonAsync<GoveeHttpState>();
        serviceResponse.Success = true;
        serviceResponse.Message = "";
        serviceResponse.Data = state;
        return serviceResponse;
    }
    /// <inheritdoc/>
    public async Task<ServiceResponse<bool>> SetOnOff(string deviceId, string deviceModel, bool on)
    {
        var serviceResponse = new ServiceResponse<bool>();

        var value = "0";
        if (on)
        {
            value = "1";
        }
        
        var jsonPayload = $@"
        {{
            ""requestId"": ""{Guid.NewGuid()}"",
            ""payload"": {{
                ""sku"": ""{deviceModel}"",
                ""device"": ""{deviceId}"",
                ""capability"": {{
                    ""type"": ""devices.capabilities.on_off"",
                    ""instance"": ""powerSwitch"",
                    ""value"": {value}
                }}
            }}
        }}";
        
        var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync($"{GoveeApiAddress}{GoveeControlEndpoint}", content);
        if (!response.IsSuccessStatusCode)
        {
            serviceResponse.Success = false;
            serviceResponse.Message = response.ReasonPhrase;
            return serviceResponse;
        }

        serviceResponse.Success = true;
        serviceResponse.Message = "";
        return serviceResponse;
    }
    /// <inheritdoc/>
    public async Task<ServiceResponse<bool>> SetColor(string deviceId, string deviceModel, RgbColor color)
    {
        var serviceResponse = new ServiceResponse<bool>();

        var value = ((color.R & 0xFF) << 16) | ((color.G & 0xFF) << 8) | (color.B & 0xFF);
        
        var jsonPayload = $@"
        {{
            ""requestId"": ""{Guid.NewGuid()}"",
            ""payload"": {{
                ""sku"": ""{deviceModel}"",
                ""device"": ""{deviceId}"",
                ""capability"": {{
                    ""type"": ""devices.capabilities.color_setting"",
                    ""instance"": ""colorRgb"",
                    ""value"": {value}
                }}
            }}
        }}";
        
        var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync($"{GoveeApiAddress}{GoveeControlEndpoint}", content);
        if (!response.IsSuccessStatusCode)
        {
            serviceResponse.Success = false;
            serviceResponse.Message = response.ReasonPhrase;
            return serviceResponse;
        }

        serviceResponse.Success = true;
        serviceResponse.Message = "";
        return serviceResponse;
    }

    public Task<ServiceResponse<bool>> SetColorTemp(string deviceId, string deviceModel, int value)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public async Task<ServiceResponse<bool>> SetBrightness(string deviceId, string deviceModel, int value)
    {
        var serviceResponse = new ServiceResponse<bool>();
        
        var jsonPayload = $@"
        {{
            ""requestId"": ""{Guid.NewGuid()}"",
            ""payload"": {{
                ""sku"": ""{deviceModel}"",
                ""device"": ""{deviceId}"",
                ""capability"": {{
                    ""type"": ""devices.capabilities.range"",
                    ""instance"": ""brightness"",
                    ""value"": {value}
                }}
            }}
        }}";
        
        var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync($"{GoveeApiAddress}{GoveeControlEndpoint}", content);
        if (!response.IsSuccessStatusCode)
        {
            serviceResponse.Success = false;
            serviceResponse.Message = response.ReasonPhrase;
            return serviceResponse;
        }

        serviceResponse.Success = true;
        serviceResponse.Message = "";
        return serviceResponse;
    }
    /// <inheritdoc/>
    public async Task<ServiceResponse<List<GoveeScene>>> GetScenes(string deviceId, string deviceModel)
    {
        var serviceResponse = new ServiceResponse<List<GoveeScene>>();
        
        var jsonPayload = $@"
        {{
            ""requestId"": ""{Guid.NewGuid()}"",
            ""payload"": {{
                ""sku"": ""{deviceModel}"",
                ""device"": ""{deviceId}""
            }}
        }}";
        
        var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync($"{GoveeApiAddress}{GoveeControlEndpoint}", content);
        if (!response.IsSuccessStatusCode)
        {
            serviceResponse.Success = false;
            serviceResponse.Message = response.ReasonPhrase;
            return serviceResponse;
        }
        // TODO Test response Content
        serviceResponse.Success = true;
        serviceResponse.Message = "";
        return serviceResponse;
    }

    /// <inheritdoc/>
    public async Task<ServiceResponse<bool>> SetLightScene(string deviceId, string deviceModel, int sceneValue)
    {
        var serviceResponse = new ServiceResponse<bool>();
        
        var jsonPayload = $@"
        {{
            ""requestId"": ""{Guid.NewGuid()}"",
            ""payload"": {{
                ""sku"": ""{deviceModel}"",
                ""device"": ""{deviceId}"",
                ""capability"": {{
                    ""type"": ""devices.capabilities.dynamic_scene"",
                    ""instance"": ""lightScene"",
                    ""value"": {sceneValue}
                }}
            }}
        }}";
        
        var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync($"{GoveeApiAddress}{GoveeControlEndpoint}", content);
        if (!response.IsSuccessStatusCode)
        {
            serviceResponse.Success = false;
            serviceResponse.Message = response.ReasonPhrase;
            return serviceResponse;
        }

        serviceResponse.Success = true;
        serviceResponse.Message = "";
        return serviceResponse;
    }
    /// <inheritdoc/>
    public async Task<ServiceResponse<bool>> SetDiyScene(string deviceId, string deviceModel, int sceneValue)
    {
        var serviceResponse = new ServiceResponse<bool>();
        
        var jsonPayload = $@"
        {{
            ""requestId"": ""{Guid.NewGuid()}"",
            ""payload"": {{
                ""sku"": ""{deviceModel}"",
                ""device"": ""{deviceId}"",
                ""capability"": {{
                    ""type"": ""devices.capabilities.dynamic_scene"",
                    ""instance"": ""diyScene"",
                    ""value"": {sceneValue}
                }}
            }}
        }}";
        
        var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync($"{GoveeApiAddress}{GoveeControlEndpoint}", content);
        if (!response.IsSuccessStatusCode)
        {
            serviceResponse.Success = false;
            serviceResponse.Message = response.ReasonPhrase;
            return serviceResponse;
        }

        serviceResponse.Success = true;
        serviceResponse.Message = "";
        return serviceResponse;
    }
}