using System.Net;
using System.Net.Sockets;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reactive.Threading.Tasks;
using System.Text;
using System.Text.Json;
using GoveeCSharpConnector.Interfaces;
using GoveeCSharpConnector.Objects;
namespace GoveeCSharpConnector.Services;

public class GoveeUdpService : IGoveeUdpService
{
    private const string GoveeMulticastAddress = "239.255.255.250";
    private const int GoveeMulticastPortListen = 4002;
    private const int GoveeMulticastPortSend = 4001;
    private readonly UdpClient _udpClient = new();
    private bool _udpListenerActive = true;

    private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
    private readonly Subject<string> _messageSubject = new();
    private readonly Subject<GoveeUdpDevice> _scanResultSubject = new();
    private readonly Subject<GoveeUdpState> _stateResultSubject = new();

    public IObservable<string> Messages => _messageSubject;

    public GoveeUdpService()
    {
        SetupUdpClientListener();
    }

    /// <inheritdoc/>
    public async Task<List<GoveeUdpDevice>> GetDevices(TimeSpan? timeout = null)
    {
        if (!_udpListenerActive)
            throw new Exception("Udp Listener not started!");
        // Block this Method until current call reaches end of Method
        await _semaphore.WaitAsync();

        try
        {
            // Build Message
            var message = new GoveeUdpMessage
            {
                msg = new msg
                {
                    cmd = "scan",
                    data = new { account_topic = "reserve" }
                }
            };
            // Subscribe to ScanResultSubject
            var devicesTask = _scanResultSubject
                .TakeUntil(Observable.Timer(timeout ?? TimeSpan.FromMilliseconds(250)))
                .ToList()
                .ToTask();

            // Send Message
            SendUdpMessage(JsonSerializer.Serialize(message), GoveeMulticastAddress, GoveeMulticastPortSend);

            // Return List
            return (await devicesTask).ToList();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        finally
        {
            // Release Method Block
            _semaphore.Release();
        }
    }

    /// <inheritdoc/>
    public async Task<GoveeUdpState> GetState(string deviceAddress, int uniCastPort = 4003, TimeSpan? timeout = null)
    {
        if (!_udpListenerActive)
            throw new Exception("Udp Listener not started!");
        try
        {
            // Build Message
            var message = new GoveeUdpMessage
            {
                msg = new msg
                {
                    cmd = "devStatus",
                    data = new { }
                }
            };
            // Subscribe to ScanResultSubject
            var devicesTask = _stateResultSubject
                .TakeUntil(Observable.Timer(timeout ?? TimeSpan.FromMilliseconds(250)))
                .ToTask();

            // Send Message
            SendUdpMessage(JsonSerializer.Serialize(message), deviceAddress, uniCastPort);

            // Return state
            return await devicesTask;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    /// <inheritdoc/>
    public async Task ToggleDevice(string deviceAddress, bool on, int uniCastPort = 4003)
    {
        try
        {
            // Build Message
            var message = new GoveeUdpMessage
            {
                msg = new msg
                {
                    cmd = "turn",
                    data = new { value = on ? 1 : 0 }
                }
            };
            // Send Message
            SendUdpMessage(JsonSerializer.Serialize(message), deviceAddress, uniCastPort);

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    /// <inheritdoc/>
    public async Task SetBrightness(string deviceAddress, int brightness, int uniCastPort = 4003)
    {
        try
        {
            // Build Message
            var message = new GoveeUdpMessage
            {
                msg = new msg
                {
                    cmd = "brightness",
                    data = new { value = brightness }
                }
            };
            // Send Message
            SendUdpMessage(JsonSerializer.Serialize(message), deviceAddress, uniCastPort);

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    /// <inheritdoc/>
    public async Task SetColor(string deviceAddress, RgbColor color, int uniCastPort = 4003)
    {
        try
        {
            // Build Message
            var message = new GoveeUdpMessage
            {
                msg = new msg
                {
                    cmd = "colorwc",
                    data = new 
                    { color = new
                        {
                            r = color.R,
                            g = color.G,
                            b = color.B
                        },
                        colorTempInKelvin = 0
                    }
                }
            };
            // Send Message
            SendUdpMessage(JsonSerializer.Serialize(message), deviceAddress, uniCastPort);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task SetColorTemp(string deviceAddress, int colorTempInKelvin, int uniCastPort = 4003)
    {
        try
        {
            // Build Message
            var message = new GoveeUdpMessage
            {
                msg = new msg
                {
                    cmd = "colorwc",
                    data = new
                    {
                        color = new
                        {
                            r = 0,
                            g = 0,
                            b = 0
                        },
                        colorTempInKelvin = colorTempInKelvin
                    }
                }
            };
            // Send Message
            SendUdpMessage(JsonSerializer.Serialize(message), deviceAddress, uniCastPort);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    /// <inheritdoc/>
    public async void StartUdpListener()
    {
        _udpListenerActive = true;
        await StartListener();
    }
    /// <inheritdoc/>
    public bool IsListening()
    {
        return _udpListenerActive;
    }
    /// <inheritdoc/>
    public void StopUdpListener()
    {
        _udpListenerActive = false;
        _udpClient.DropMulticastGroup(IPAddress.Parse(GoveeMulticastAddress));
        _udpClient.Close();
    }

    private static void SendUdpMessage(string message, string receiverAddress, int receiverPort)
    {
        var client = new UdpClient();
        try
        {
            byte[] data = Encoding.UTF8.GetBytes(message);
            client.Send(data, data.Length, receiverAddress, receiverPort);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        finally
        {
            client.Close();
        }
    }

    private async void SetupUdpClientListener()
    {
        _udpClient.ExclusiveAddressUse = false;
        var localEndPoint = new IPEndPoint(IPAddress.Any, GoveeMulticastPortListen);
        _udpClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
        _udpClient.Client.Bind(localEndPoint);
        await StartListener();
    }

    private async Task StartListener()
    {
        try
        {
            _udpClient.JoinMulticastGroup(IPAddress.Parse(GoveeMulticastAddress));

            Task.Run(async () =>
            {
                while (_udpListenerActive)
                {
                    var remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
                    var data = _udpClient.Receive(ref remoteEndPoint);

                    var message = Encoding.UTF8.GetString(data);

                    UdPMessageReceived(message);
                    _messageSubject.OnNext(message);
                }
            });
        }
        catch(Exception ex)
        {
            throw ex;
        }
    }

    private void UdPMessageReceived(string message)
    {
        var response = JsonSerializer.Deserialize<GoveeUdpMessage>(message);
        switch (response.msg.cmd)
        {
            case "scan":
                var device = JsonSerializer.Deserialize<GoveeUdpDevice>(response.msg.data.ToString());
                _scanResultSubject.OnNext(device);
                break;
            case "devStatus":
                var state = JsonSerializer.Deserialize<GoveeUdpState>(response.msg.data.ToString());
                _stateResultSubject.OnNext(state);
                break;
        }
    }
}