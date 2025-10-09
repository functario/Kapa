using Kapa.Abstractions.Rules;

namespace Kapa.Fixtures.Rules;

public sealed class GenericRule : IRule
{
    public string Name => nameof(GenericRule);

    public Type TypeOfT => typeof(object);

    public IOutcome Validate<T>(T subject)
    {
        return TypedOutcomes.Ok(Name);
    }
}
