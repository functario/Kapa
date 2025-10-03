namespace Kapa.Fixtures.Capabilities.Inheritances;

[CapabilityType]
public class ChildLevel2CapabilityType : ChildLevel1CapabilityType
{
    [Capability(nameof(Handle2))]
    public IOutcome Handle2()
    {
        return new Outcome(nameof(OneCapability), OutcomeStatus.Ok);
    }
}
