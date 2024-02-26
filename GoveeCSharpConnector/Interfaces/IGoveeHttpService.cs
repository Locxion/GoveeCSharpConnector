using GoveeCSharpConnector.Objects;
using GoveeCSharpConnector.Objects.Misc;

namespace GoveeCSharpConnector.Interfaces;

public interface IGoveeHttpService
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
    /// <returns>List of GoveeHttpDevices</returns>
    Task<ServiceResponse<List<GoveeHttpDevice>>> GetDevices();
    /// <summary>
    /// Requests the State of a single Govee Device
    /// </summary>
    /// <param name="deviceId">Device Id Guid as string</param>
    /// <param name="deviceModel">Device Model Number as string</param>
    /// <returns>GoveeHttpState Object</returns>
    Task<ServiceResponse<GoveeHttpState>> GetDeviceState(string deviceId, string deviceModel);
    /// <summary>
    /// Sets the On/Off state of a single Govee Device
    /// </summary>
    /// <param name="deviceId">Device Id Guid as string</param>
    /// <param name="deviceModel">Device Model Number as string</param>
    /// <param name="on"></param>
    /// <returns></returns>
    Task<ServiceResponse<bool>> SetOnOff(string deviceId, string deviceModel, bool on);
    /// <summary>
    /// Sets a Rgb Color of a single Govee Device
    /// </summary>
    /// <param name="deviceId">Device Id Guid as string</param>
    /// <param name="deviceModel">Device Model Number as string</param>
    /// <param name="color">Rgb Color</param>
    /// <returns></returns>
    Task<ServiceResponse<bool>> SetColor(string deviceId, string deviceModel, RgbColor color);
    /// <summary>
    /// Sets the Color Temperature of a single Govee Device
    /// </summary>
    /// <param name="deviceId">Device Id Guid as string</param>
    /// <param name="deviceModel">Device Model Number as string</param>
    /// <param name="value">Color Temp in Kelvin as Int</param>
    /// <returns></returns>
    Task<ServiceResponse<bool>> SetColorTemp(string deviceId, string deviceModel, int value);
    /// <summary>
    /// Sets the Brightness of a single Govee Device
    /// </summary>
    /// <param name="deviceId">Device Id Guid as string</param>
    /// <param name="deviceModel">Device Model Number as string</param>
    /// <param name="value">Value 1-100</param>
    /// <returns></returns>
    Task<ServiceResponse<bool>> SetBrightness(string deviceId, string deviceModel, int value);

    /// <summary>
    /// Gets a List of all available Govee Scenes for the Device
    /// </summary>
    /// <param name="deviceId">Device Id Guid as string</param>
    /// <param name="deviceModel">Device Model Number as string</param>
    /// <returns></returns>
    Task<ServiceResponse<List<GoveeScene>>> GetScenes(string deviceId, string deviceModel);
    /// <summary>
    /// Sets the LightScene of a single Govee Device
    /// </summary>
    /// <param name="deviceId">Device Id Guid as string</param>
    /// <param name="deviceModel">Device Model Number as string</param>
    /// <param name="sceneValue">Number of the Scene</param>
    /// <returns></returns>
    Task<ServiceResponse<bool>> SetLightScene(string deviceId, string deviceModel, int sceneValue);
    /// <summary>
    /// Sets the DiyScene of a single Govee Device
    /// </summary>
    /// <param name="deviceId">Device Id Guid as string</param>
    /// <param name="deviceModel">Device Model Number as string</param>
    /// <param name="sceneValue">Number of the Scene</param>
    /// <returns></returns>
    Task<ServiceResponse<bool>> SetDiyScene(string deviceId, string deviceModel, int sceneValue);
}