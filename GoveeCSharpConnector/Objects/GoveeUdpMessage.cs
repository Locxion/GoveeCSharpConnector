// ReSharper disable InconsistentNaming
namespace GoveeCSharpConnector.Objects;

public class GoveeUdpMessage
{
    public msg msg { get; set; }
}
public class msg
{
    public string cmd { get; set; }
    public object data { get; set; }
}