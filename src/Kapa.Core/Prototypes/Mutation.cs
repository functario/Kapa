using System.Linq.Expressions;
using Kapa.Abstractions.Prototypes;

namespace Kapa.Core.Prototypes;

public sealed record Mutation<TGeneratedPrototype>(Expression<Func<TGeneratedPrototype, bool>> MutationExpression)
    : IMutation<TGeneratedPrototype>
    where TGeneratedPrototype : IGeneratedPrototype
{
    public Func<TGeneratedPrototype, bool> CompiledMutation => MutationExpression.Compile();
}
