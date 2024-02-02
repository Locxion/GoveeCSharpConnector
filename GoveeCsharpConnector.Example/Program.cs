using System.Net.Mime;
using System.Reflection;
using System.Xml.Linq;
using GoveeCSharpConnector.Objects;
using GoveeCSharpConnector.Services;

namespace GoveeCsharpConnector.Example;

public class Program
{
    private static GoveeApiService _goveeApiService = new GoveeApiService();
    public static List<GoveeApiDevice> _apiDevices = new List<GoveeApiDevice>();

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
                _apiDevices = await _goveeApiService.GetDevices();
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
                    await _goveeApiService.ToggleState(_apiDevices.First(x => x.DeviceName.ToLower() == nameInput).DeviceId, _apiDevices.First(x => x.DeviceName.ToLower() == nameInput).Model, true);
                }
                else
                {
                    await _goveeApiService.ToggleState(_apiDevices.First(x => x.DeviceName.ToLower() == nameInput).DeviceId, _apiDevices.First(x => x.DeviceName.ToLower() == nameInput).Model, false);
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

                await _goveeApiService.SetBrightness(_apiDevices.First(x => x.DeviceName.ToLower() == nameInput2).DeviceId, _apiDevices.First(x => x.DeviceName.ToLower() == nameInput2).Model, value);
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
                        await _goveeApiService.SetColor(_apiDevices.First(x => x.DeviceName.ToLower() == nameInput3).DeviceId, model, new RgbColor(0, 0 ,254));
                        break;
                    case "green":
                        await _goveeApiService.SetColor(_apiDevices.First(x => x.DeviceName.ToLower() == nameInput3).DeviceId, model, new RgbColor(0, 254 ,0));
                        break;
                    case "red":
                        await _goveeApiService.SetColor(_apiDevices.First(x => x.DeviceName.ToLower() == nameInput3).DeviceId, model, new RgbColor(254, 0 ,0));
                        break;
                }
                Console.WriteLine($"Set Color of Device {nameInput3} to {colorInput}!");
                EndSegment();
                break;
        }
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
            _goveeApiService.SetApiKey(input);
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
        if (string.IsNullOrEmpty(_goveeApiService.GetApiKey()))
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