namespace Kapa.Fixtures.Capabilities.Inheritances.Statics;

[CapabilityType]
public class StaticChildLevel2CapabilityType : ChildLevel1CapabilityType
{
    [Capability(nameof(Handle2))]
    public IOutcome Handle2()
    {
        return TypedOutcomes.Ok(nameof(StaticChildLevel2CapabilityType));
    }
}
