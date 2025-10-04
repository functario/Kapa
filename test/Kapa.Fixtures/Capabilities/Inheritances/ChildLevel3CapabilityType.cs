using Kapa.Abstractions.Results;

namespace Kapa.Fixtures.Capabilities.Inheritances;

[CapabilityType]
public class ChildLevel3CapabilityType : ChildLevel2CapabilityType
{
    [Capability(nameof(Handle3))]
    public IOutcome Handle3()
    {
        return new Outcome(nameof(ChildLevel3CapabilityType), OutcomeStatus.Ok);
    }
}
