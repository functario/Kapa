namespace Kapa.Fixtures.Capabilities;

[CapabilityType]
public sealed class OneCapability
{
    [Capability("Represents a person action")]
    public IOutcome Handle()
    {
        return new Outcome(nameof(OneCapability), OutcomeStatus.Ok);
    }
}
