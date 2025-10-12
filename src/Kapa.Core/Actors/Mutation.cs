using Kapa.Abstractions.Actors;

namespace Kapa.Core.Actors;

public sealed record Mutation<TGeneratedActor>(
    string Description,
    Func<IGeneratedActor, bool> Predicate
) : IMutation<TGeneratedActor>
    where TGeneratedActor : IGeneratedActor
{ }
