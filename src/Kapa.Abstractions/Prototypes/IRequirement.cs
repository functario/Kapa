using System.Linq.Expressions;

namespace Kapa.Abstractions.Prototypes;

public interface IRequirement<TGeneratedPrototype>
    where TGeneratedPrototype : IGeneratedPrototype
{
    public Expression<Func<TGeneratedPrototype, bool>> ConditionExpression { get; }
    public HashSet<string> ReferencedProperties { get; }
}
