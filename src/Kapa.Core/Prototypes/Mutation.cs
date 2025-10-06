using System.Linq.Expressions;
using Kapa.Abstractions.Prototypes;

namespace Kapa.Core.Prototypes;

public sealed record Mutation<THasTrait>(Expression<Func<THasTrait, object?>> MutationExpression)
    : IMutation<THasTrait>
    where THasTrait : IHasTrait
{
    public Func<THasTrait, object?> CompiledMutation => MutationExpression.Compile();
}
