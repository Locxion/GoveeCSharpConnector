namespace GoveeCSharpConnector.Objects;

public class RgbColor
{
    public short R { get; set; }
    public short G { get; set; }
    public short B { get; set; }

    public RgbColor(int r, int g, int b)
    {
        R = Convert.ToInt16(r);
        G = Convert.ToInt16(g);
        B = Convert.ToInt16(b);
    }
}