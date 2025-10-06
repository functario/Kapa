using System.Linq.Expressions;
using Kapa.Abstractions.Prototypes;

namespace Kapa.Core.Prototypes;

public sealed record Requirement<TPrototype>(Expression<Func<TPrototype, bool>> ConditionExpression)
    : IRequirement<TPrototype>
    where TPrototype : IPrototype
{
    public Func<TPrototype, bool> CompiledCondition => ConditionExpression.Compile();

    // Extract all property names referenced in the condition (computed once)
    public HashSet<string> ReferencedProperties { get; } = ExtractPropertyNames(ConditionExpression);

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
            // Check if this is accessing a member from the parameter
            // It could be: p.Property or ((CastType)p).Property
            var current = node.Expression;
            while (current != null)
            {
                if (current.NodeType == ExpressionType.Parameter)
                {
                    Properties.Add(node.Member.Name);
                    break;
                }
                // Handle casts: ((Type)p).Property
                if (current is UnaryExpression unary && unary.NodeType == ExpressionType.Convert)
                {
                    current = unary.Operand;
                }
                else
                {
                    break;
                }
            }
            return base.VisitMember(node);
        }
    }
}
