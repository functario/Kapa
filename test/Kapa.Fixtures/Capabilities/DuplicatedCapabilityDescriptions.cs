namespace Kapa.Fixtures.Capabilities;

[CapabilityType]
public sealed class DuplicatedCapabilityDescriptions
{
    private const string SameDescription = nameof(SameDescription);

    [Capability(nameof(SameDescription))]
    public IOutcome Handle1()
    {
        return new Outcome(nameof(OneCapability), OutcomeStatus.Ok);
    }

    [Capability(nameof(SameDescription))]
    public IOutcome Handle2()
    {
        return new Outcome(nameof(OneCapability), OutcomeStatus.Ok);
    }

    [Capability(nameof(SameDescription))]
    public IOutcome Handle3()
    {
        return new Outcome(nameof(OneCapability), OutcomeStatus.Ok);
    }
}
