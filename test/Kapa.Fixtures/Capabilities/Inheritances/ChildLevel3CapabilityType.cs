using Kapa.Abstractions.Validations;
using Kapa.Core.Validations;

namespace Kapa.Fixtures.Capabilities.Inheritances;

[CapabilityType]
public class ChildLevel3CapabilityType : ChildLevel2CapabilityType
{
    [Capability(nameof(Handle3))]
    public IOutcome Handle3()
    {
        return TypedOutcomes.Ok(nameof(ChildLevel3CapabilityType));
    }
}
