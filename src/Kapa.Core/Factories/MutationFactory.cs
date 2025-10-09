using System.Linq.Expressions;
using Kapa.Abstractions.Actors;
using Kapa.Core.Actors;

namespace Kapa.Core.Factories;

public static class MutationFactory
{
    public static Mutation<IGeneratedActor> Create<T>(this Expression<Func<T, bool>> expr)
        where T : IGeneratedActor
    {
        ArgumentNullException.ThrowIfNull(expr);
        ValidateMutationIsSimple(expr);

        // Rebuild the expression tree with IGeneratedActor parameter
        var parameter = Expression.Parameter(typeof(IGeneratedActor), "p");
        var castParameter = Expression.Convert(parameter, typeof(T));
        var visitor = new ParameterReplacerVisitor(expr.Parameters[0], castParameter);
        var newBody = visitor.Visit(expr.Body);
        var convertedExpr = Expression.Lambda<Func<IGeneratedActor, bool>>(newBody, parameter);

        return new Mutation<IGeneratedActor>(convertedExpr);
    }

    private static void ValidateMutationIsSimple<T>(Expression<Func<T, bool>> expr)
    {
        var body = expr.Body;

        // Remove Convert/Cast
        while (body is UnaryExpression unary && unary.NodeType == ExpressionType.Convert)
        {
            body = unary.Operand;
        }

        // Check for logical operators
        if (body is BinaryExpression binary)
        {
            if (
                binary.NodeType == ExpressionType.AndAlso
                || binary.NodeType == ExpressionType.OrElse
            )
            {
                throw new InvalidOperationException(
                    $"Mutation expressions must be simple (single comparison without &&, ||). Expression: {expr}"
                );
            }
        }
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
