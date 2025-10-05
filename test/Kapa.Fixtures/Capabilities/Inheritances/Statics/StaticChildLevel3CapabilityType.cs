using Kapa.Abstractions.Validations;
using Kapa.Core.Validations;

namespace Kapa.Fixtures.Capabilities.Inheritances.Statics;

[CapabilityType]
public class StaticChildLevel3CapabilityType : StaticChildLevel2CapabilityType
{
    [Capability(nameof(Handle3))]
    public IOutcome Handle3()
    {
        return TypedOutcomes.Ok(nameof(StaticChildLevel3CapabilityType));
    }
}
