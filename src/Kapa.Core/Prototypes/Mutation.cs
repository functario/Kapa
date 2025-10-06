using System.Linq.Expressions;
using Kapa.Abstractions.Prototypes;

namespace Kapa.Core.Prototypes;

public sealed record Mutation<TPrototype>(
    Expression<Func<TPrototype, object?>> PropertyExpression,
    Expression<Action<TPrototype>> MutationExpression
) : IMutation<TPrototype>
    where TPrototype : IPrototype, new()
{
    public Action<TPrototype> CompiledMutation => MutationExpression.Compile();

    // Extract the property name being mutated
    public string MutatedProperty => ExtractPropertyName(PropertyExpression);

    private static string ExtractPropertyName(Expression expression)
    {
        if (expression is LambdaExpression lambda)
            expression = lambda.Body;

        // Handle boxing conversions for value types
        if (expression is UnaryExpression unary && unary.NodeType == ExpressionType.Convert)
            expression = unary.Operand;

        if (expression is MemberExpression member)
            return member.Member.Name;

        throw new ArgumentException("Expression must be a property access");
    }
}
