using Kapa.Abstractions.Validations;
using Kapa.Core.Validations;
using Kapa.Fixtures.Capabilities.NoParameters;

namespace Kapa.Fixtures.Capabilities.Inheritances.Statics;

[CapabilityType]
public class StaticParentCapabitlity
{
    [Capability(nameof(Handle))]
    public IOutcome Handle()
    {
        return TypedOutcomes.Ok(nameof(OneCapability));
    }
}
