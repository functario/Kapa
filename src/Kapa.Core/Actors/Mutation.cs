using System.Linq.Expressions;
using Kapa.Abstractions.Actors;

namespace Kapa.Core.Actors;

public sealed record Mutation<TGeneratedActor>(
    Expression<Func<TGeneratedActor, bool>> MutationExpression
) : IMutation<TGeneratedActor>
    where TGeneratedActor : IGeneratedActor
{
    public Func<TGeneratedActor, bool> CompiledMutation => MutationExpression.Compile();
}
