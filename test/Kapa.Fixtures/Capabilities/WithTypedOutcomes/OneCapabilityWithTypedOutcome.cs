using Kapa.Abstractions;

namespace Kapa.Fixtures.Capabilities.WithTypedOutcomes;

[CapabilityType]
public sealed class OneCapabilityWithTypedOutcome
{
    [Capability(nameof(Handle))]
    public IOutcome Handle()
    {
        return new TypedOutcome<Kinds>(
            nameof(OneCapabilityWithTypedOutcome),
            OutcomeStatus.Ok,
            Kinds.StringKind,
            ""
        );
    }
}
