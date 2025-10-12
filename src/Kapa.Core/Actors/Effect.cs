using Kapa.Abstractions.Actors;

namespace Kapa.Core.Actors;

public sealed record Effect<TGeneratedActor>(
    string Description,
    Func<IGeneratedActor, bool> Predicate
) : IEffect<TGeneratedActor>
    where TGeneratedActor : IGeneratedActor
{ }
