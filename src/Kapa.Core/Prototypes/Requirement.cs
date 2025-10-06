using System.Linq.Expressions;
using Kapa.Abstractions.Prototypes;

namespace Kapa.Core.Prototypes;

public sealed record Requirement<TPrototype>(Expression<Func<TPrototype, bool>> ConditionExpression)
    : IRequirement<TPrototype>
    where TPrototype : IPrototype, new()
{
    public Func<TPrototype, bool> CompiledCondition => ConditionExpression.Compile();

    // Extract all property names referenced in the condition
    public HashSet<string> ReferencedProperties => ExtractPropertyNames(ConditionExpression);

    private static HashSet<string> ExtractPropertyNames(Expression expression)
    {
        var visitor = new PropertyVisitor();
        visitor.Visit(expression);
        return visitor.Properties;
    }

    // Visitor to extract property names from expression tree
    private sealed class PropertyVisitor : ExpressionVisitor
    {
        public HashSet<string> Properties { get; } = [];

        protected override Expression VisitMember(MemberExpression node)
        {
            if (node.Member.DeclaringType != null)
                Properties.Add(node.Member.Name);
            return base.VisitMember(node);
        }
    }
}
