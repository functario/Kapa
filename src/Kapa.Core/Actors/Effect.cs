using Kapa.Abstractions.Actors;

namespace Kapa.Core.Actors;

public sealed record Effect<TGeneratedActor>(
    string Id,
    string Description,
    Func<IGeneratedActor, bool> Predicate
) : IEffect<TGeneratedActor>
    where TGeneratedActor : IGeneratedActor
{
    public bool AreEqual(IEffect<IGeneratedActor> other) =>
        Id.Equals(other?.Id, StringComparison.OrdinalIgnoreCase);
}
