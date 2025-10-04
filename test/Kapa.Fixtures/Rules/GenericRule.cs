using Kapa.Abstractions.Results;
using Kapa.Abstractions.Rules;

namespace Kapa.Fixtures.Rules;

public sealed class GenericRule : IRule
{
    public string Name => nameof(GenericRule);

    public IOutcome Validate()
    {
        return new Outcome(Name, OutcomeStatus.Ok);
    }
}
