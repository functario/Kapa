namespace Kapa.Fixtures.Capabilities.Inheritances;

[CapabilityType]
public class ChildLevel1CapabilityType : ParentCapabitlity
{
    [Capability(nameof(Handle1))]
    public IOutcome Handle1()
    {
        return new Outcome(nameof(ChildLevel1CapabilityType), OutcomeStatus.Ok);
    }
}
