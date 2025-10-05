using Kapa.Abstractions.Validations;
using Kapa.Core.Validations;

namespace Kapa.Fixtures.Capabilities.WithTypedOutcomes;

[CapabilityType]
public sealed class OkOutcomeCapacity
{
    [Capability(nameof(Handle))]
    public IOutcome Handle()
    {
        return TypedOutcomes.Ok(nameof(OkOutcomeCapacity));
    }
}
