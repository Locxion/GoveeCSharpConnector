namespace GoveeCSharpConnector.Objects;

public class GoveeApiCommand
{
    public string Device { get; set; }
    public string Model { get; set; }
    public Command Cmd { get; set; }
}

public class Command
{
    public string Name { get; set; }
    public object Value { get; set; }
}