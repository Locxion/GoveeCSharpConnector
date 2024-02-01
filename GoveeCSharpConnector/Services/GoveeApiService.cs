using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using GoveeCSharpConnector.Interfaces;
using GoveeCSharpConnector.Objects;

namespace GoveeCSharpConnector.Services;

public class GoveeApiService : IGoveeApiService
{
    private string _apiKey = string.Empty;
    private const string GoveeApiAddress = "https://developer-api.govee.com/v1";
    private readonly HttpClient _httpClient = new();

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
        _httpClient.DefaultRequestHeaders.Remove("Govee-Api-Key");
    }
    /// <inheritdoc/>
    public async Task<List<GoveeApiDevice>> GetDevices()
    {
        var response = await _httpClient.GetFromJsonAsync<GoveeResponse>($"{GoveeApiAddress}/devices");
        
        return response.Data.Devices;
    }
    /// <inheritdoc/>
    public async Task<GoveeApiState> GetDeviceState(string deviceId, string deviceModel)
    {
        return await _httpClient.GetFromJsonAsync<GoveeApiState>($"{GoveeApiAddress}/devices/state?device={deviceId}&model={deviceModel}");
    }
    /// <inheritdoc/>
    public async Task ToggleState(string deviceId, string deviceModel, bool on)
    {
        await SendCommand(deviceId, deviceModel, "turn", on ? "on" : "off");
    }
    /// <inheritdoc/>
    public async Task SetBrightness(string deviceId, string deviceModel, int value)
    {
        await SendCommand(deviceId, deviceModel, "brightness", value);
    }
    /// <inheritdoc/>
    public async Task SetColor(string deviceId, string deviceModel, RgbColor color)
    {
        await SendCommand(deviceId, deviceModel, "color", color);
    }
    /// <inheritdoc/>
    public async Task SetColorTemp(string deviceId, string deviceModel, int value)
    {
        await SendCommand(deviceId, deviceModel, "colorTem", value);
    }

    private async Task SendCommand(string deviceId, string deviceModel, string command, object commandObject)
    {
        var commandRequest = new GoveeApiCommand()
        {
            Device = deviceId,
            Model = deviceModel,
            Cmd = new Command()
            {
                Name = command,
                Value = commandObject
            }
        };
        var httpContent = new StringContent(JsonSerializer.Serialize(commandRequest), Encoding.UTF8, "application/json");
        var response = await _httpClient.PutAsync($"{GoveeApiAddress}/devices/control", httpContent);
        if (!response.IsSuccessStatusCode)
            throw new Exception($"Govee Api Request failed. Status code: {response.StatusCode}, Message: {response.Content}");
    }
}