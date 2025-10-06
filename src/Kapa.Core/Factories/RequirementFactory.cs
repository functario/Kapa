using System.Linq.Expressions;
using Kapa.Abstractions.Prototypes;
using Kapa.Core.Prototypes;

namespace Kapa.Core.Factories;

public static class RequirementFactory
{
    public static Requirement<IGeneratedPrototype> Create<T>(this Expression<Func<T, bool>> expr)
        where T : IGeneratedPrototype
    {
        ArgumentNullException.ThrowIfNull(expr);
        ValidateRequirementIsSimple(expr);
        
        // Rebuild the expression tree with IGeneratedPrototype parameter
        var parameter = Expression.Parameter(typeof(IGeneratedPrototype), "p");
        var castParameter = Expression.Convert(parameter, typeof(T));
        var visitor = new ParameterReplacerVisitor(expr.Parameters[0], castParameter);
        var newBody = visitor.Visit(expr.Body);
        var convertedExpr = Expression.Lambda<Func<IGeneratedPrototype, bool>>(newBody, parameter);
        
        return new Requirement<IGeneratedPrototype>(convertedExpr);
    }

    private static void ValidateRequirementIsSimple<T>(Expression<Func<T, bool>> expr)
    {
        var body = expr.Body;
        
        // Remove Convert/Cast
        while (body is UnaryExpression unary && unary.NodeType == ExpressionType.Convert)
        {
            body = unary.Operand;
        }
        
        // Check for logical operators
        if (ContainsLogicalOperators(body))
        {
            throw new InvalidOperationException(
                $"Requirement expressions must be simple (single comparison without &&, ||). Expression: {expr}");
        }
    }

    private static bool ContainsLogicalOperators(Expression expression)
    {
        if (expression is BinaryExpression binary)
        {
            if (binary.NodeType == ExpressionType.AndAlso || binary.NodeType == ExpressionType.OrElse)
            {
                return true;
            }
            
            // Check recursively in case there are nested expressions
            return ContainsLogicalOperators(binary.Left) || ContainsLogicalOperators(binary.Right);
        }

        if (expression is UnaryExpression unary)
        {
            return ContainsLogicalOperators(unary.Operand);
        }

        return false;
    }

    private sealed class ParameterReplacerVisitor : ExpressionVisitor
    {
        private readonly ParameterExpression _oldParameter;
        private readonly Expression _newParameter;

        public ParameterReplacerVisitor(ParameterExpression oldParameter, Expression newParameter)
        {
            _oldParameter = oldParameter;
            _newParameter = newParameter;
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            return node == _oldParameter ? _newParameter : base.VisitParameter(node);
        }
    }
}
