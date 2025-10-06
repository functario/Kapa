namespace Kapa.Fixtures.Capabilities.Inheritances;

[CapabilityType]
public class ChildLevel1CapabilityType : ParentCapabitlity
{
    [Capability(nameof(Handle1))]
    public IOutcome Handle1()
    {
        return TypedOutcomes.Ok(nameof(ChildLevel1CapabilityType));
    }
}
