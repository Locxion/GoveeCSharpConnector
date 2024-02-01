using GoveeCSharpConnector.Objects;

namespace GoveeCSharpConnector.Interfaces;

public interface IGoveeApiService
{
    /// <summary>
    /// Sets the required Api Key for the Govee Api.
    /// Request Api Key in the Mobile Phone App.
    /// </summary>
    /// <param name="apiKey">Api Key as String</param>
    void SetApiKey(string apiKey);
    /// <summary>
    /// Returns current set Govee Api Key
    /// </summary>
    /// <returns>Govee Api Key as String</returns>
    string GetApiKey();
    /// <summary>
    /// Removes the Set Api Key and resets the HTTP Header
    /// </summary>
    void RemoveApiKey();
    /// <summary>
    /// Requests all Devices registered to Api Key Govee Account
    /// </summary>
    /// <returns>List of GoveeApiDevices</returns>
    Task<List<GoveeApiDevice>> GetDevices();
    /// <summary>
    /// Requests the State of a single Govee Device
    /// </summary>
    /// <param name="deviceId">Device Id Guid as string</param>
    /// <param name="deviceModel">Device Model Number as string</param>
    /// <returns>GoveeApiStat Object</returns>
    public Task<GoveeApiState> GetDeviceState(string deviceId, string deviceModel);
    /// <summary>
    /// Sets the On/Off state of a single Govee Device
    /// </summary>
    /// <param name="deviceId">Device Id Guid as string</param>
    /// <param name="deviceModel">Device Model Number as string</param>
    /// <param name="on"></param>
    /// <returns></returns>
    public Task ToggleState(string deviceId, string deviceModel, bool on);
    /// <summary>
    /// Sets the Brightness in Percent of a single Govee Device
    /// </summary>
    /// <param name="deviceId">Device Id Guid as string</param>
    /// <param name="deviceModel">Device Model Number as string</param>
    /// <param name="value">Brightness in Percent as Int</param>
    /// <returns></returns>
    public Task SetBrightness(string deviceId, string deviceModel, int value);
    /// <summary>
    /// Sets a Rgb Color of a single Govee Device
    /// </summary>
    /// <param name="deviceId">Device Id Guid as string</param>
    /// <param name="deviceModel">Device Model Number as string</param>
    /// <param name="color">Rgb Color</param>
    /// <returns></returns>
    public Task SetColor(string deviceId, string deviceModel, RgbColor color);
    /// <summary>
    /// Sets the Color Temperature of a single Govee Device
    /// </summary>
    /// <param name="deviceId">Device Id Guid as string</param>
    /// <param name="deviceModel">Device Model Number as string</param>
    /// <param name="value">Color Temp in Kelvin as Int</param>
    /// <returns></returns>
    public Task SetColorTemp(string deviceId, string deviceModel, int value);
    
}