// ReSharper disable InconsistentNaming
namespace GoveeCSharpConnector.Objects;

public class GoveeUdpDevice
{
    public string ip { get; set; }
    public string device { get; set; }
    public string sku { get; set; }
    public string bleVersionHard { get; set; }
    public string bleVersionSoft { get; set; }
    public string wifiVersionHard { get; set; }
    public string wifiVersionSoft { get; set; }
}