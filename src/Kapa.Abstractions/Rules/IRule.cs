using Kapa.Abstractions.Outcomes;

namespace Kapa.Abstractions.Rules;

public interface IRule
{
    public string Name { get; }
    public IOutcome Validate();
}
