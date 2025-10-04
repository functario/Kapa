using Kapa.Abstractions.Results;

namespace Kapa.Fixtures.Capabilities.NoParameters;

[CapabilityType]
public static class StaticCapabilityType
{
    [Capability(nameof(Handle))]
    public static IOutcome Handle()
    {
        return new Outcome(nameof(StaticCapabilityType), OutcomeStatus.Ok);
    }
}
