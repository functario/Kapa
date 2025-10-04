using Kapa.Abstractions.Rules;

namespace Kapa.Fixtures.Rules;

public sealed class GenericRule : IRule
{
    public string Name => nameof(GenericRule);

    public IOutcome Validate()
    {
        return TypedOutcomes.Ok(Name);
    }
}
