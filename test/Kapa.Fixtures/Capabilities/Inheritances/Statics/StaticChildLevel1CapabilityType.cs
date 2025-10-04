namespace Kapa.Fixtures.Capabilities.Inheritances.Statics;

[CapabilityType]
public class StaticChildLevel1CapabilityType : StaticParentCapabitlity
{
    [Capability(nameof(Handle1))]
    public IOutcome Handle1()
    {
        return TypedOutcomes.Ok(nameof(StaticChildLevel1CapabilityType));
    }
}
