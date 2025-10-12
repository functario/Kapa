using Kapa.Abstractions.Actors;
using Kapa.Core.Factories;

namespace Kapa.Core.Extensions;

public static class PredicateExtensions
{
    public static IEffect<TGeneratedActor> ToEffect<TGeneratedActor>(
        this Predicate<TGeneratedActor> predicate,
        string id,
        string description
    )
        where TGeneratedActor : IGeneratedActor
    {
        ArgumentNullException.ThrowIfNull(predicate);
        ArgumentNullException.ThrowIfNullOrWhiteSpace(id);

        return EffectFactory.Create(predicate, id, description);
    }
}
