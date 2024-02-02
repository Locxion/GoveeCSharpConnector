using System.Data;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using GoveeCSharpConnector.Objects;

namespace GoveeCSharpConnector.Interfaces;

public interface IGoveeUdpService
{
    /// <summary>
    /// Sends a Scan Command via Udp Multicast.
    /// </summary>
    /// <param name="timeout">Standard 200ms</param>
    /// <returns>List of GoveeUdpDevices</returns>
    Task<List<GoveeUdpDevice>> GetDevices(TimeSpan? timeout = null);

    /// <summary>
    /// Request the State of the Device
    /// </summary>
    /// <param name="deviceAddress">Ip Address of the Device</param>
    /// <param name="uniCastPort">Port of the Device. Standard 4003</param>
    /// <param name="timeout">Standard 200ms</param>
    /// <returns></returns>
    Task<GoveeUdpState> GetState(string deviceAddress, int uniCastPort = 4003, TimeSpan? timeout = null);

    /// <summary>
    /// Sets the On/Off State of the Device
    /// </summary>
    /// <param name="deviceAddress">Ip Address of the Device</param>
    /// <param name="on"></param>
    /// <param name="uniCastPort">Port of the Device. Standard 4003</param>
    /// <returns></returns>
    Task ToggleDevice(string deviceAddress, bool on, int uniCastPort = 4003);
    /// <summary>
    /// Sets the Brightness of the Device
    /// </summary>
    /// <param name="deviceAddress">Ip Address of the Device</param>
    /// <param name="brightness">In Percent 1-100</param>
    /// <param name="uniCastPort">Port of the Device. Standard 4003</param>
    /// <returns></returns>
    Task SetBrightness(string deviceAddress, int brightness, int uniCastPort = 4003);
    /// <summary>
    /// Sets the Color of the Device
    /// </summary>
    /// <param name="deviceAddress">Ip Address of the Device</param>
    /// <param name="color"></param>
    /// <param name="uniCastPort">Port of the Device. Standard 4003</param>
    /// <returns></returns>
    Task SetColor(string deviceAddress, RgbColor color, int uniCastPort = 4003);
    /// <summary>
    /// Starts the Udp Listener
    /// </summary>
    /// <returns></returns>
    void StartUdpListener();
    /// <summary>
    /// Returns the State of the Udp Listener
    /// </summary>
    /// <returns>True if Active</returns>
    bool IsListening();
    /// <summary>
    /// Stops the Udp Listener
    /// </summary>
    /// <returns></returns>
    void StopUdpListener();
}
