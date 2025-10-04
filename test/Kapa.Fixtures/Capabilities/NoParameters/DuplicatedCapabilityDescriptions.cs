using Kapa.Abstractions.Results;

namespace Kapa.Fixtures.Capabilities.NoParameters;

[CapabilityType]
public sealed class DuplicatedCapabilityDescriptions
{
    private const string SameDescription = nameof(SameDescription);

    [Capability(nameof(SameDescription))]
    public IOutcome Handle1()
    {
        return new Outcome(nameof(DuplicatedCapabilityDescriptions), OutcomeStatus.Ok);
    }

    [Capability(nameof(SameDescription))]
    public IOutcome Handle2()
    {
        return new Outcome(nameof(DuplicatedCapabilityDescriptions), OutcomeStatus.Ok);
    }

    [Capability(nameof(SameDescription))]
    public IOutcome Handle3()
    {
        return new Outcome(nameof(DuplicatedCapabilityDescriptions), OutcomeStatus.Ok);
    }
}
