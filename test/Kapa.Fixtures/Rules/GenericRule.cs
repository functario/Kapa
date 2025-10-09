using Kapa.Abstractions.Actors;
using Kapa.Abstractions.Rules;

namespace Kapa.Fixtures.Rules;

public sealed class GenericRule : IRule
{
    public string Name => nameof(GenericRule);

    public Type TypeOfT => typeof(object);

    public IOutcome Validate<TActor, TSubject>(TActor _, TSubject __)
        where TActor : IActor
    {
        return TypedOutcomes.Ok(Name);
    }
}
