using System.Linq.Expressions;

namespace Kapa.Abstractions.Prototypes;

public interface IRequirement<TPrototype>
    where TPrototype : IPrototype
{
    public Expression<Func<TPrototype, bool>> ConditionExpression { get; }
    public HashSet<string> ReferencedProperties { get; }
}
