using System.Linq.Expressions;
using Kapa.Abstractions.Prototypes;

namespace Kapa.Core.Prototypes;

public sealed record Mutation<TPrototype>(Expression<Func<TPrototype, object?>> MutationExpression)
    : IMutation<TPrototype>
    where TPrototype : IPrototype
{
    public Func<TPrototype, object?> CompiledMutation => MutationExpression.Compile();
}
