using System.Linq.Expressions;

namespace Kapa.Abstractions.Actors;

public interface IMutation<TGeneratedActor>
    where TGeneratedActor : IGeneratedActor
{
    public Expression<Func<TGeneratedActor, bool>> MutationExpression { get; }
    Func<TGeneratedActor, bool> CompiledMutation { get; }
}
