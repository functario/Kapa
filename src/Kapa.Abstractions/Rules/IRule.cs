using Kapa.Abstractions.Outcomes;

namespace Kapa.Abstractions.Rules;

/// <summary>
/// A rule that can be validated.
/// </summary>
public interface IRule
{
    public string Name { get; }
    public IOutcome Validate();
}
