using Kapa.Abstractions.Results;

namespace Kapa.Fixtures.Capabilities.NoParameters;

// Intentionally missing [CapabilityType] attribute for testing
public sealed class NotCapabilityType
{
    public IOutcome Handle()
    {
        return new Outcome(nameof(NotCapabilityType), OutcomeStatus.Ok);
    }
}
