namespace HomeAutomation.Actors.Identifications;

public sealed class Identification
{
    public bool IsAuthenticated { get; set; }

    public Token? Token { get; set; }
}
