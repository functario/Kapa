namespace Kapa.Fixtures.Capabilities;

[CapabilityType]
public sealed class OneCapability
{
    [Capability(nameof(Handle))]
    public IOutcome Handle()
    {
        return new Outcome(nameof(OneCapability), OutcomeStatus.Ok);
    }
}
