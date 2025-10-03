namespace Kapa.Fixtures.Capabilities;

[CapabilityType]
public sealed class ManyCapabilities
{
    [Capability(nameof(Handle1))]
    public IOutcome Handle1()
    {
        return new Outcome(nameof(ManyCapabilities), OutcomeStatus.Ok);
    }

    [Capability(nameof(Handle2))]
    public IOutcome Handle2()
    {
        return new Outcome(nameof(ManyCapabilities), OutcomeStatus.Ok);
    }

    [Capability(nameof(Handle3))]
    public IOutcome Handle3()
    {
        return new Outcome(nameof(ManyCapabilities), OutcomeStatus.Ok);
    }
}
