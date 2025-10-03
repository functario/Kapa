namespace Kapa.Fixtures.Capabilities;

[CapabilityType]
public static class StaticCapabilityType
{
    [Capability(nameof(Handle))]
    public static IOutcome Handle()
    {
        return new Outcome(nameof(OneCapability), OutcomeStatus.Ok);
    }
}
