using GoveeCSharpConnector.Interfaces;
using GoveeCSharpConnector.Objects;

namespace GoveeCSharpConnector.Services;

public class GoveeService : IGoveeService
{
    public string GoveeApiKey { get; set; }

    private readonly IGoveeApiService _apiService;
    private readonly IGoveeUdpService _udpService;

    public GoveeService(IGoveeApiService apiService,IGoveeUdpService udpService)
    {
        _apiService = apiService ?? throw new ArgumentNullException(nameof(apiService));
        _udpService = udpService ?? throw new ArgumentNullException(nameof(udpService));
    }
    public async Task<List<GoveeDevice>> GetDevices(bool onlyLan = true)
    {
        if (string.IsNullOrWhiteSpace(GoveeApiKey)) throw new Exception("No Govee Api Key Set!");
        var apiDevices = await _apiService.GetDevices();
        var devices = apiDevices.Select(apiDevice => new GoveeDevice() { DeviceId = apiDevice.DeviceId, DeviceName = apiDevice.DeviceName, Model = apiDevice.Model, Address = "onlyAvailableOnUdpRequest" }).ToList();
        if (!onlyLan)
            return devices;

        if (!_udpService.IsListening())
            _udpService.StartUdpListener();
        
        var udpDevices = await _udpService.GetDevices();

        var combinedDevices = (from goveeDevice in devices let matchingDevice = udpDevices.FirstOrDefault(x => x.device == goveeDevice.DeviceId)
            where matchingDevice is not null select 
                new GoveeDevice { DeviceId = goveeDevice.DeviceId, DeviceName = goveeDevice.DeviceName, Model = goveeDevice.Model, Address = matchingDevice.ip }).ToList();

        return combinedDevices;
    }

    public async Task<GoveeState> GetDeviceState(GoveeDevice goveeDevice, bool useUdp = true)
    {
        if (useUdp)
        {
            if (!_udpService.IsListening())
                _udpService.StartUdpListener();
            if (string.IsNullOrWhiteSpace(goveeDevice.Address)) throw new Exception("Device not available via Udp/Lan");
            var udpState = await _udpService.GetState(goveeDevice.Address);
            return new GoveeState() { State = udpState.onOff, Brightness = udpState.brightness, Color = udpState.color, ColorTempInKelvin = udpState.colorTempInKelvin };
        }
        if (string.IsNullOrWhiteSpace(GoveeApiKey)) throw new Exception("No Govee Api Key Set!");
        var apiState = await _apiService.GetDeviceState(goveeDevice.DeviceId, goveeDevice.Model);
        return new GoveeState{State = apiState.Properties.PowerState, Brightness = apiState.Properties.Brightness, Color = apiState.Properties.Color, ColorTempInKelvin = apiState.Properties.ColorTemp};
    }

    public async Task ToggleState(GoveeDevice goveeDevice, bool on, bool useUdp = true)
    {
        if (useUdp)
        {
            if (string.IsNullOrWhiteSpace(goveeDevice.Address)) throw new Exception("Device not available via Udp/Lan");
            await _udpService.ToggleDevice(goveeDevice.Address, on);
            return;
        }
        if (string.IsNullOrWhiteSpace(GoveeApiKey)) throw new Exception("No Govee Api Key Set!");
        await _apiService.ToggleState(goveeDevice.DeviceId, goveeDevice.Model, on);
    }

    public async Task SetBrightness(GoveeDevice goveeDevice, int value, bool useUdp = true)
    {
        if (useUdp)
        {
            if (string.IsNullOrWhiteSpace(goveeDevice.Address)) throw new Exception("Device not available via Udp/Lan");
            await _udpService.SetBrightness(goveeDevice.Address, value);
            return;
        }
        if (string.IsNullOrWhiteSpace(GoveeApiKey)) throw new Exception("No Govee Api Key Set!");
        await _apiService.SetBrightness(goveeDevice.DeviceId, goveeDevice.Model, value);
    }

    public async Task SetColor(GoveeDevice goveeDevice, RgbColor color, bool useUdp = true)
    {
        if (useUdp)
        {
            if (string.IsNullOrWhiteSpace(goveeDevice.Address)) throw new Exception("Device not available via Udp/Lan");
            await _udpService.SetColor(goveeDevice.Address, color);
            return;
        }
        if (string.IsNullOrWhiteSpace(GoveeApiKey)) throw new Exception("No Govee Api Key Set!");
        await _apiService.SetColor(goveeDevice.DeviceId, goveeDevice.Model, color);
    }

    public async Task SetColorTemp(GoveeDevice goveeDevice, int value, bool useUdp = true)
    {
        if (useUdp)
        {
            if (string.IsNullOrWhiteSpace(goveeDevice.Address)) throw new Exception("Device not available via Udp/Lan");
            await _udpService.SetColorTemp(goveeDevice.Address, value);
            return;
        }
        if (string.IsNullOrWhiteSpace(GoveeApiKey)) throw new Exception("No Govee Api Key Set!");
        await _apiService.SetColorTemp(goveeDevice.DeviceId, goveeDevice.Model, value);
    }
}