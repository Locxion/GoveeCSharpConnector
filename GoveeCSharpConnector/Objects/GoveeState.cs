using GoveeCSharpConnector.Enums;

namespace GoveeCSharpConnector.Objects;

public class GoveeState
{
    public PowerState State { get; set; }
    public int Brightness { get; set; }
    public RgbColor Color { get; set; }
    public int ColorTempInKelvin { get; set; }
}