namespace Kapa.Fixtures.Capabilities.Inheritances.Statics;

[CapabilityType]
public class StaticChildLevel3CapabilityType : StaticChildLevel2CapabilityType
{
    [Capability(nameof(Handle3))]
    public IOutcome Handle3()
    {
        return new Outcome(nameof(StaticChildLevel3CapabilityType), OutcomeStatus.Ok);
    }
}
