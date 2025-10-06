using System.Linq.Expressions;

namespace Kapa.Abstractions.Prototypes;

public interface IMutation<TPrototype>
    where TPrototype : IPrototype
{
    public Expression<Func<TPrototype, object?>> MutationExpression { get; }
    Func<TPrototype, object?> CompiledMutation { get; }
}
