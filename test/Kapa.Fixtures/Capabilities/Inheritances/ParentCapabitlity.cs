namespace Kapa.Fixtures.Capabilities.Inheritances;

[CapabilityType]
public class ParentCapabitlity
{
    [Capability(nameof(Handle))]
    public IOutcome Handle()
    {
        return new Outcome(nameof(OneCapability), OutcomeStatus.Ok);
    }
}
