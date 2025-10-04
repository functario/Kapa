using Kapa.Fixtures.Capabilities.NoParameters;

namespace Kapa.Fixtures.Capabilities.Inheritances.Statics;

[CapabilityType]
public class StaticParentCapabitlity
{
    [Capability(nameof(Handle))]
    public IOutcome Handle()
    {
        return new Outcome(nameof(OneCapability), OutcomeStatus.Ok);
    }
}
