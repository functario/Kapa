using Kapa.Abstractions.Validations;
using Kapa.Core.Validations;

namespace Kapa.Fixtures.Capabilities.Inheritances;

[CapabilityType]
public class ParentCapabitlity
{
    [Capability(nameof(Handle))]
    public IOutcome Handle()
    {
        return TypedOutcomes.Ok(nameof(ParentCapabitlity));
    }
}
