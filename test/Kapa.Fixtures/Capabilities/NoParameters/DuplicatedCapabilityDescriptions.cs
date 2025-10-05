using Kapa.Abstractions.Validations;
using Kapa.Core.Validations;

namespace Kapa.Fixtures.Capabilities.NoParameters;

[CapabilityType]
public sealed class DuplicatedCapabilityDescriptions
{
    private const string SameDescription = nameof(SameDescription);

    [Capability(nameof(SameDescription))]
    public IOutcome Handle1()
    {
        return TypedOutcomes.Ok(nameof(DuplicatedCapabilityDescriptions));
    }

    [Capability(nameof(SameDescription))]
    public IOutcome Handle2()
    {
        return TypedOutcomes.Ok(nameof(DuplicatedCapabilityDescriptions));
    }

    [Capability(nameof(SameDescription))]
    public IOutcome Handle3()
    {
        return TypedOutcomes.Ok(nameof(DuplicatedCapabilityDescriptions));
    }
}
