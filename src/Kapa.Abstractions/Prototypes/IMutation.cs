using System.Linq.Expressions;

namespace Kapa.Abstractions.Prototypes;

public interface IMutation<THasTrait>
    where THasTrait : IHasTrait
{
    public Expression<Func<THasTrait, object?>> MutationExpression { get; }
    Func<THasTrait, object?> CompiledMutation { get; }
}
