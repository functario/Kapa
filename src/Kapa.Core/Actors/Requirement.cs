using Kapa.Abstractions.Actors;

namespace Kapa.Core.Actors;

public sealed record Requirement<TGeneratedActor>(
    string Description,
    Func<IGeneratedActor, bool> Predicate
) : IRequirement<TGeneratedActor>
    where TGeneratedActor : IGeneratedActor
{ }
