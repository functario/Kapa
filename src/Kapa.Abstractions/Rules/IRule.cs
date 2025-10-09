using Kapa.Abstractions.Actors;
using Kapa.Abstractions.Validations;

namespace Kapa.Abstractions.Rules;

/// <summary>
/// A rule that can be validated.
/// <see cref="IRule"/> need a default constructor.
/// </summary>
public interface IRule
{
    public string Name { get; }
    public IOutcome Validate<TActor, TSubject>(TActor actor, TSubject subject)
        where TActor : IActor;
    public Type TypeOfT { get; }
}
