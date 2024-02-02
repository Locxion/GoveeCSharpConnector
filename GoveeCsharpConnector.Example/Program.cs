using System.Net.Mime;
using System.Reflection;
using System.Xml.Linq;
using GoveeCSharpConnector.Objects;
using GoveeCSharpConnector.Services;

namespace GoveeCsharpConnector.Example;

public class Program
{
    private static readonly GoveeApiService GoveeApiService = new ();
    private static readonly GoveeUdpService GoveeUdpService = new ();
    private static List<GoveeApiDevice> _apiDevices = new ();
    private static List<GoveeUdpDevice> _udpDevices = new();

    public static async Task Main(string[] args)
    {
        while (true)
        {
            PrintWelcomeMessage();
            var input = Console.ReadLine();
            HandleKeyInput(input);
        }
    }

    private static async void HandleKeyInput(string input)
    {
        switch (input)
        {
            case "1":
                HandleApiInput();
                EndSegment();
                break;
            case "2":
                Console.WriteLine("Requesting Devices ...");
                _apiDevices = await GoveeApiService.GetDevices();
                Console.WriteLine("Devices:");
                foreach (var device in _apiDevices)
                {
                    Console.WriteLine($"Name: {device.DeviceName}, Device Id: {device.DeviceId}, Model: {device.Model}, Controllable {device.Controllable}");
                }
                Console.WriteLine($"Total: {_apiDevices.Count} Devices.");
                EndSegment();
            break;
            case "3":
                if (_apiDevices.Count == 0)
                {
                    Console.WriteLine("No Devices discovered! Please use Option 2 first!");
                    EndSegment();
                    return;
                }
                Console.WriteLine("Please enter the Name of the Device:");
                var nameInput = Console.ReadLine()?.ToLower();
                if (string.IsNullOrWhiteSpace(nameInput) || _apiDevices.FirstOrDefault(x => x.DeviceName.ToLower() == nameInput) is null)
                {
                   Console.WriteLine("Device Name Invalid!");
                   EndSegment();
                   return;
                }

                Console.WriteLine($"Do you want to turn the Device {nameInput} on or off?");
                var onOffInput = Console.ReadLine()?.ToLower();
                if (string.IsNullOrWhiteSpace(onOffInput) || (onOffInput != "on" && onOffInput != "off"))
                {
                    Console.WriteLine("Invalid Input!");
                    EndSegment();
                    return;
                }

                if (input == "on")
                {
                    await GoveeApiService.ToggleState(_apiDevices.First(x => x.DeviceName.ToLower() == nameInput).DeviceId, _apiDevices.First(x => x.DeviceName.ToLower() == nameInput).Model, true);
                }
                else
                {
                    await GoveeApiService.ToggleState(_apiDevices.First(x => x.DeviceName.ToLower() == nameInput).DeviceId, _apiDevices.First(x => x.DeviceName.ToLower() == nameInput).Model, false);
                }
                EndSegment();
                break;
            case "4":
                if (_apiDevices.Count == 0)
                {
                    Console.WriteLine("No Devices discovered! Please use Option 2 first!");
                    EndSegment();
                    return;
                }
                Console.WriteLine("Please enter the Name of the Device:");
                var nameInput2 = Console.ReadLine()?.ToLower();
                if (string.IsNullOrWhiteSpace(nameInput2) || _apiDevices.FirstOrDefault(x => x.DeviceName.ToLower() == nameInput2) is null)
                {
                    Console.WriteLine("Device Name Invalid!");
                    EndSegment();
                    return;
                }

                Console.WriteLine($"Please enter a Brightness Value for Device {nameInput2}. 0-100");
                var brightnessInput = Console.ReadLine();
                int value = Convert.ToInt16(brightnessInput);
                if (string.IsNullOrWhiteSpace(brightnessInput) || value < 0 || value > 100)
                {
                    Console.WriteLine("Invalid Input!");
                    EndSegment();
                    return;
                }

                await GoveeApiService.SetBrightness(_apiDevices.First(x => x.DeviceName.ToLower() == nameInput2).DeviceId, _apiDevices.First(x => x.DeviceName.ToLower() == nameInput2).Model, value);
                Console.WriteLine($"Set Brightness of Device {nameInput2} to {value}%!");
                EndSegment();
                break;
            case "5":
                if (_apiDevices.Count == 0)
                {
                    Console.WriteLine("No Devices discovered! Please use Option 2 first!");
                    EndSegment();
                    return;
                }
                Console.WriteLine("Please enter the Name of the Device:");
                var nameInput3 = Console.ReadLine()?.ToLower();
                if (string.IsNullOrWhiteSpace(nameInput3) || _apiDevices.FirstOrDefault(x => x.DeviceName.ToLower() == nameInput3) is null)
                {
                    Console.WriteLine("Device Name Invalid!");
                    EndSegment();
                    return;
                }
                Console.WriteLine($"Please choose a Color to set {nameInput3} to ... (blue, red, green)");
                var colorInput = Console.ReadLine()?.ToLower();
                if (string.IsNullOrWhiteSpace(colorInput) || (colorInput != "blue" && colorInput != "green" && colorInput != "red"))
                {
                    Console.WriteLine("Invalid Input!");
                    EndSegment();
                    return;
                }

                var model = _apiDevices.FirstOrDefault(x => x.DeviceName.ToLower()== nameInput3)?.Model;
                switch (colorInput)
                {
                    case "blue":
                        await GoveeApiService.SetColor(_apiDevices.First(x => x.DeviceName.ToLower() == nameInput3).DeviceId, model, new RgbColor(0, 0 ,254));
                        break;
                    case "green":
                        await GoveeApiService.SetColor(_apiDevices.First(x => x.DeviceName.ToLower() == nameInput3).DeviceId, model, new RgbColor(0, 254 ,0));
                        break;
                    case "red":
                        await GoveeApiService.SetColor(_apiDevices.First(x => x.DeviceName.ToLower() == nameInput3).DeviceId, model, new RgbColor(254, 0 ,0));
                        break;
                }
                Console.WriteLine($"Set Color of Device {nameInput3} to {colorInput}!");
                EndSegment();
                break;
            case "6":
                Console.WriteLine("Requesting Devices ...");
                _udpDevices = await GoveeUdpService.GetDevices();
                Console.WriteLine("Devices:");
                foreach (var device in _udpDevices)
                {
                    Console.WriteLine($"IpAddress: {device.ip}, Device Id: {device.device}, Model: {device.sku}");
                }
                Console.WriteLine($"Total: {_udpDevices.Count} Devices.");
                EndSegment();
                break;
            case "7":
                var selectedDevice = GetUdpDeviceSelection();

                Console.WriteLine($"Do you want to turn the Device {selectedDevice.ip} on or off?");
                var onOffInput2 = Console.ReadLine()?.ToLower();
                if (string.IsNullOrWhiteSpace(onOffInput2) || (onOffInput2 != "on" && onOffInput2 != "off"))
                {
                    Console.WriteLine("Invalid Input!");
                    EndSegment();
                    return;
                }

                if (input == "on")
                {
                    await GoveeUdpService.ToggleDevice(selectedDevice.ip, true);
                }
                else
                {
                    await GoveeUdpService.ToggleDevice(selectedDevice.ip, false);
                }
                EndSegment();
                break;
            case "8":
                var selectedDevice2 = GetUdpDeviceSelection();

                Console.WriteLine($"Please enter a Brightness Value for Device {selectedDevice2.ip}. 0-100");
                var brightnessInput2 = Console.ReadLine();
                int value2 = Convert.ToInt16(brightnessInput2);
                if (string.IsNullOrWhiteSpace(brightnessInput2) || value2 < 0 || value2 > 100)
                {
                    Console.WriteLine("Invalid Input!");
                    EndSegment();
                    return;
                }

                await GoveeUdpService.SetBrightness(selectedDevice2.ip, value2);                
                Console.WriteLine($"Set Brightness of Device {selectedDevice2.ip} to {value2}%!");
                EndSegment();
                break;
            case "9":
                var selectedDevice3 = GetUdpDeviceSelection();
                Console.WriteLine($"Please choose a Color to set {selectedDevice3.ip} to ... (blue, red, green)");
                var colorInput2 = Console.ReadLine()?.ToLower();
                if (string.IsNullOrWhiteSpace(colorInput2) || (colorInput2 != "blue" && colorInput2 != "green" && colorInput2 != "red"))
                {
                    Console.WriteLine("Invalid Input!");
                    EndSegment();
                    return;
                }

                switch (colorInput2)
                {
                    case "blue":
                        GoveeUdpService.SetColor(selectedDevice3.ip, new RgbColor(0, 0, 254));
                        break;
                    case "green":
                        GoveeUdpService.SetColor(selectedDevice3.ip, new RgbColor(0, 254, 0));
                        break;
                    case "red":
                        GoveeUdpService.SetColor(selectedDevice3.ip, new RgbColor(254, 0, 0));
                        break;
                }
                Console.WriteLine($"Set Color of Device {selectedDevice3.ip} to {colorInput2}!");
                EndSegment();
                break;
        }
    }

    private static GoveeUdpDevice GetUdpDeviceSelection()
    {
        var count = 1;
        Console.WriteLine("Please Choose a Device from the List:");
        foreach (var device in _udpDevices)
        {
            Console.WriteLine($"{count} - IpAdress: {device.ip}, Device Id {device.device}, Model {device.sku}");
            count++;
        }

        var input = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(input) || Int16.TryParse(input, out var result) is false)
        {
            Console.WriteLine("Invalid Input!");
            return GetUdpDeviceSelection();
        }

        return _udpDevices[result];
    }

    private static void HandleApiInput()
    {
        while (true)
        {
            Console.WriteLine("Please enter/paste your Govee Api Key ...");
            Console.WriteLine("Your Api Key should look something like this: xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx");
            var input = Console.ReadLine();
            if (input is null || input.Length != 36)
            {
                Console.WriteLine("Wrong Api Key Format!");
                continue;
            }
            GoveeApiService.SetApiKey(input);
            break;
        }
        Console.WriteLine("Api Key saved!");
    }

    private static void EndSegment()
    {
        Console.WriteLine("---------------------------Press any Key to continue---------------------------");
        Console.ReadLine();
    }

    private static void PrintWelcomeMessage()
    {
        Console.WriteLine();
        Console.WriteLine("Welcome to the GoveeCSharpConnector Example!");
        Console.WriteLine($"Version: {Assembly.GetEntryAssembly()?.GetName().Version}");
        Console.WriteLine($"To test/explore the GoveeCSharpConnector Version: {Assembly.Load("GoveeCSharpConnector").GetName().Version}");
        Console.WriteLine("----------------------------------------------------------");
        if (string.IsNullOrEmpty(GoveeApiService.GetApiKey()))
        {
            Console.WriteLine("1 - Enter GoveeApi Key - START HERE (Required for Api Service Options!)");
        }
        else
        {
            Console.WriteLine("1 - Enter GoveeApi Key - Already Set!");
            Console.WriteLine("Api Service:");
            Console.WriteLine("2 - Get a List of all Devices connected to the Api Key Account");
            Console.WriteLine("3 - Turn Device On or Off");
            Console.WriteLine("4 - Set Brightness for Device");
            Console.WriteLine("5 - Set Color of Device");
        }
        Console.WriteLine("Udp Service - No Api Key needed!");
        Console.WriteLine("6 - Get a List of all Devices available in the Network");
        Console.WriteLine("7 - Turn Device On or Off");
        Console.WriteLine("8 - Set Brightness for Device");
        Console.WriteLine("9 - Set Color of Device");
    }
}