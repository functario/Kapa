using System.Linq.Expressions;

namespace Kapa.Abstractions.Prototypes;

public interface IMutation<TPrototype>
    where TPrototype : IPrototype, new()
{
    public Expression<Func<TPrototype, object?>> PropertyExpression { get; }
    public Expression<Action<TPrototype>> MutationExpression { get; }
    public string MutatedProperty { get; }
}
