using System.Linq.Expressions;

namespace Kapa.Abstractions.Prototypes;

public interface IRequirement<THasTrait>
    where THasTrait : IGeneratedPrototype
{
    public Expression<Func<THasTrait, bool>> ConditionExpression { get; }
    public HashSet<string> ReferencedProperties { get; }
}
