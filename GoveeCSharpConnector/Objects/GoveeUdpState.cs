using GoveeCSharpConnector.Enums;

namespace GoveeCSharpConnector.Objects;

public class GoveeUdpState
{
    public PowerState onOff { get; set; }
    public short brightness { get; set; }
    public RgbColor color { get; set; }
    public int colorTempInKelvin { get; set; }
}