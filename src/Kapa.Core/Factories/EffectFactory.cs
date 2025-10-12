using Kapa.Abstractions.Actors;
using Kapa.Core.Actors;

namespace Kapa.Core.Factories;

public static class EffectFactory
{
    public static Effect<TGeneratedActor> Create<TGeneratedActor>(
        this Func<TGeneratedActor, bool> predicate,
        string id,
        string description
    )
        where TGeneratedActor : IGeneratedActor
    {
        ArgumentNullException.ThrowIfNull(predicate);
        ArgumentNullException.ThrowIfNullOrWhiteSpace(id);
        return new Effect<TGeneratedActor>(
            id,
            description,
            actor => predicate((TGeneratedActor)actor)
        );
    }
}
