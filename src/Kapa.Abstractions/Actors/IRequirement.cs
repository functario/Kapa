using System.Linq.Expressions;

namespace Kapa.Abstractions.Actors;

public interface IRequirement<TGeneratedActor>
    where TGeneratedActor : IGeneratedActor
{
    public Expression<Func<TGeneratedActor, bool>> ConditionExpression { get; }
    public HashSet<string> ReferencedProperties { get; }
}
