using System.Linq.Expressions;

namespace Kapa.Abstractions.Prototypes;

public interface IMutation<TGeneratedPrototype>
    where TGeneratedPrototype : IGeneratedPrototype
{
    public Expression<Func<TGeneratedPrototype, bool>> MutationExpression { get; }
    Func<TGeneratedPrototype, bool> CompiledMutation { get; }
}
