namespace Kapa.Fixtures.Capabilities;

// Intentionally missing [CapabilityType] attribute for testing
public sealed class NotCapabilityType
{
    public IOutcome HandleAction()
    {
        return new Outcome(nameof(NotCapabilityType), OutcomeStatus.Ok);
    }
}
