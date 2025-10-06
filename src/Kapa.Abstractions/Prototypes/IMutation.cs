using System.Linq.Expressions;

namespace Kapa.Abstractions.Prototypes;

public interface IMutation<THasTrait>
    where THasTrait : IHasTrait
{
    public Expression<Func<THasTrait, bool>> MutationExpression { get; }
    Func<THasTrait, bool> CompiledMutation { get; }
}
