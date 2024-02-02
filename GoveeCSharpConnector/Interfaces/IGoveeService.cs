using GoveeCSharpConnector.Objects;

namespace GoveeCSharpConnector.Interfaces;

public interface IGoveeService
{
    /// <summary>
    /// Govee Api Key
    /// </summary>
    string GoveeApiKey { get; set; }

    /// <summary>
    /// Gets a List of Govee Devices
    /// </summary>
    /// <param name="onlyLan">If true returns that are available on Api and Lan</param>
    /// <returns>List of Govee Devices</returns>
    Task<List<GoveeDevice>> GetDevices(bool onlyLan = true);

    /// <summary>
    /// Gets the State of a GoveeDevice
    /// </summary>
    /// <param name="goveeDevice">GoveeDevice</param>
    /// <param name="useUdp">Use Udp Connection instead of the Api</param>
    /// <returns></returns>
    Task<GoveeState> GetDeviceState(GoveeDevice goveeDevice, bool useUdp = true);

    /// <summary>
    /// Sets the On/Off State of the GoveeDevice
    /// </summary>
    /// <param name="goveeDevice">GoveeDevice</param>
    /// <param name="on"></param>
    /// <param name="useUdp">Use Udp Connection instead of the Api</param>
    /// <returns></returns>
    Task ToggleState(GoveeDevice goveeDevice, bool on, bool useUdp = true);

    /// <summary>
    /// Sets the Brightness of the GoveeDevice
    /// </summary>
    /// <param name="goveeDevice">GoveeDevice</param>
    /// <param name="value">Brightness in Percent</param>
    /// <param name="useUdp">Use Udp Connection instead of the Api</param>
    /// <returns></returns>
    Task SetBrightness(GoveeDevice goveeDevice, int value, bool useUdp = true);

    /// <summary>
    /// Sets the Color of the GoveeDevice
    /// </summary>
    /// <param name="goveeDevice">GoveeDevice</param>
    /// <param name="color">RgBColor</param>
    /// <param name="useUdp">Use Udp Connection instead of the Api</param>
    /// <returns></returns>
    Task SetColor(GoveeDevice goveeDevice, RgbColor color, bool useUdp = true);

    /// <summary>
    /// Sets the Color Temperature in Kelvin for the GoveeDevice
    /// </summary>
    /// <param name="goveeDevice">GoveeDevice</param>
    /// <param name="value">Color Temp in Kelvin</param>
    /// <param name="useUdp">Use Udp Connection instead of the Api</param>
    /// <returns></returns>
    Task SetColorTemp(GoveeDevice goveeDevice, int value, bool useUdp = true);
}