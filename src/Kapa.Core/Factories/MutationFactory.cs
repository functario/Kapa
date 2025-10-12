using Kapa.Abstractions.Actors;
using Kapa.Core.Actors;

namespace Kapa.Core.Factories;

public static class MutationFactory
{
    public static Mutation<TGeneratedActor> Create<TGeneratedActor>(
        this Func<TGeneratedActor, bool> predicate,
        string description
    )
        where TGeneratedActor : IGeneratedActor
    {
        ArgumentNullException.ThrowIfNull(predicate);
        return new Mutation<TGeneratedActor>(
            description,
            actor => predicate((TGeneratedActor)actor)
        );
    }
}
