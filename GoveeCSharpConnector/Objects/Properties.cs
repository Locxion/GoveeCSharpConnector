using GoveeCSharpConnector.Enums;

namespace GoveeCSharpConnector.Objects;

public class Properties
{
    public bool Online { get; set; }
    public PowerState PowerState { get; set; }
    public int Brightness { get; set; }
    public int? ColorTemp { get; set; }
    public RgbColor Color { get; set; }
}