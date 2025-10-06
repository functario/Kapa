using System.Linq.Expressions;
using Kapa.Abstractions.Prototypes;

namespace Kapa.Core.Prototypes;

public sealed record Mutation<THasTrait>(Expression<Func<THasTrait, bool>> MutationExpression)
    : IMutation<THasTrait>
    where THasTrait : IGeneratedPrototype
{
    public Func<THasTrait, bool> CompiledMutation => MutationExpression.Compile();
}
