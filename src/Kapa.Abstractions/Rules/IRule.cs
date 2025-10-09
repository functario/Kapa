using Kapa.Abstractions.Validations;

namespace Kapa.Abstractions.Rules;

/// <summary>
/// A rule that can be validated.
/// </summary>
public interface IRule
{
    public string Name { get; }
    public IOutcome Validate<T>(T subject);
    public Type TypeOfT { get; }
}
