using Kapa.Abstractions.Results;
using Kapa.Core.Results;

namespace Kapa.Fixtures.Capabilities.WithTypedOutcomes;

[CapabilityType]
public sealed class OkOutcomeCapacity
{
    [Capability(nameof(Handle))]
    public IOutcome Handle()
    {
        return TypedOutcomes.Ok(nameof(SringOutcomeCapacity));
    }
}
