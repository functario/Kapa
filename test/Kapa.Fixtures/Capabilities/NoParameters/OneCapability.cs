using Kapa.Abstractions.Validations;
using Kapa.Core.Validations;

namespace Kapa.Fixtures.Capabilities.NoParameters;

[CapabilityType]
public sealed class OneCapability
{
    [Capability(nameof(Handle))]
    public IOutcome Handle()
    {
        return TypedOutcomes.Ok(nameof(OneCapability));
    }
}
