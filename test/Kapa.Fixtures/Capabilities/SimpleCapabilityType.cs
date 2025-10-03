namespace Kapa.Fixtures.Capabilities;

[CapabilityType]
public sealed class SimpleCapabilityType
{
    [Capability("Represents a person action")]
    public IOutcome Handle()
    {
        return new Outcome(nameof(SimpleCapabilityType), OutcomeStatus.Ok);
    }
}
