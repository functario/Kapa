using System.Globalization;
using System.Text;
using Kapa.Abstractions.Actors;
using Kapa.Abstractions.Graphs;

namespace Kapa.Core.Extensions;

public static class GraphExtensions
{
    public static string ToMermaidGraph(this IGraph graph, MermaidGraphOptions? options = null)
    {
        ArgumentNullException.ThrowIfNull(graph);
        options ??= new MermaidGraphOptions();

        var sb = new StringBuilder();
        sb.AppendLine("graph TD");

        // Track which nodes we've already processed to avoid duplicates
        var processedEdges = new HashSet<string>();
        var nodesInEdges = new HashSet<string>();

        foreach (var node in graph.Nodes)
        {
            var nodeName = GetNodeName(node, options);
            var relations = node.Capability.Relations;

            // For each requirement, find nodes that can satisfy it and create edges
            if (relations?.Requirements != null && relations.Requirements.Count > 0)
            {
                foreach (var requirement in relations.Requirements)
                {
                    var requirementLabel = GetExpressionLabel(
                        requirement.ConditionExpression.Body,
                        options
                    );
                    var satisfyingNodes = FindNodesThatSatisfyRequirement(graph, requirement);

                    foreach (var satisfyingNode in satisfyingNodes)
                    {
                        var satisfyingNodeName = GetNodeName(satisfyingNode, options);
                        var edgeKey = $"{satisfyingNodeName}-->{nodeName}";

                        if (!processedEdges.Contains(edgeKey))
                        {
                            sb.AppendLine(
                                CultureInfo.InvariantCulture,
                                $"{satisfyingNodeName} -->|{requirementLabel}| {nodeName}"
                            );
                            processedEdges.Add(edgeKey);
                            nodesInEdges.Add(satisfyingNodeName);
                            nodesInEdges.Add(nodeName);
                        }
                    }

                    // If no satisfying nodes found, show the requirement is unmet
                    if (satisfyingNodes.Count == 0)
                    {
                        sb.AppendLine(
                            CultureInfo.InvariantCulture,
                            $"???[Missing] -->|{requirementLabel}| {nodeName}"
                        );
                        nodesInEdges.Add(nodeName);
                    }
                }
            }
        }

        // Add standalone nodes that were not part of any edges
        foreach (var node in graph.Nodes)
        {
            var nodeName = GetNodeName(node, options);
            if (!nodesInEdges.Contains(nodeName))
            {
                sb.AppendLine(CultureInfo.InvariantCulture, $"{nodeName}");
            }
        }

        return sb.ToString();
    }

    private static string GetNodeName(INode node, MermaidGraphOptions options)
    {
        var source = node.Capability.OutcomeMetadata.Source;

        // Extract just the capability name if not using full name
        if (!options.UseFullName)
        {
            // Get the name after the last dot (e.g., "Namespace.CapabilityName" -> "CapabilityName")
            var lastDotIndex = source.LastIndexOf('.');
            if (lastDotIndex >= 0 && lastDotIndex < source.Length - 1)
            {
                source = source[(lastDotIndex + 1)..];
            }
        }

        // Remove only parentheses and angle brackets (dots are allowed in Mermaid)
        return source
            .Replace("<", "", StringComparison.Ordinal)
            .Replace(">", "", StringComparison.Ordinal)
            .Replace("(", "", StringComparison.Ordinal)
            .Replace(")", "", StringComparison.Ordinal)
            .Replace(" ", "", StringComparison.Ordinal);
    }

    private static string GetExpressionLabel(
        System.Linq.Expressions.Expression expression,
        MermaidGraphOptions options
    )
    {
        // Remove Convert/Cast to get cleaner expression
        while (
            expression is System.Linq.Expressions.UnaryExpression unary
            && unary.NodeType == System.Linq.Expressions.ExpressionType.Convert
        )
        {
            expression = unary.Operand;
        }

        // Extract the actor type name and build a readable expression
        string expressionString;

        if (expression is System.Linq.Expressions.BinaryExpression binaryExpr)
        {
            // Unwrap any Convert expressions from the left side, but keep track of the target type
            var leftExpr = binaryExpr.Left;
            Type? convertTargetType = null;

            while (
                leftExpr is System.Linq.Expressions.UnaryExpression leftUnary
                && leftUnary.NodeType == System.Linq.Expressions.ExpressionType.Convert
            )
            {
                // Capture the target type before unwrapping
                if (convertTargetType == null)
                {
                    convertTargetType = leftUnary.Type;
                }
                leftExpr = leftUnary.Operand;
            }

            // Also check if the member expression itself has a Convert-wrapped expression
            if (leftExpr is System.Linq.Expressions.MemberExpression memberExpr)
            {
                var memberInnerExpr = memberExpr.Expression;

                // Unwrap Convert from the member's expression and capture the target type
                while (
                    memberInnerExpr is System.Linq.Expressions.UnaryExpression innerUnary
                    && innerUnary.NodeType == System.Linq.Expressions.ExpressionType.Convert
                )
                {
                    // Capture the target type of the Convert operation
                    if (convertTargetType == null)
                    {
                        convertTargetType = innerUnary.Type;
                    }
                    memberInnerExpr = innerUnary.Operand;
                }

                // Determine which type to use
                Type? typeToUse = null;
                if (convertTargetType != null)
                {
                    typeToUse = convertTargetType;
                }
                else if (memberInnerExpr is System.Linq.Expressions.ParameterExpression paramExpr)
                {
                    typeToUse = paramExpr.Type;
                }

                if (typeToUse != null)
                {
                    var fullTypeName = typeToUse.Name;

                    // Apply the same UseFullName logic as GetNodeName
                    if (!options.UseFullName)
                    {
                        var lastDotIndex = fullTypeName.LastIndexOf('.');
                        if (lastDotIndex >= 0 && lastDotIndex < fullTypeName.Length - 1)
                        {
                            fullTypeName = fullTypeName[(lastDotIndex + 1)..];
                        }
                    }

                    // Build the expression
                    var propertyName = memberExpr.Member.Name;
                    var operatorSymbol = GetOperatorSymbol(binaryExpr.NodeType);
                    var value = GetConstantValue(binaryExpr.Right);

                    expressionString = $"{fullTypeName}.{propertyName} {operatorSymbol} {value}";
                }
                else
                {
                    // Fallback to ToString if we can't parse the structure
                    expressionString = expression.ToString();
                }
            }
            else
            {
                // Fallback to ToString if we can't parse the structure
                expressionString = expression.ToString();
            }
        }
        else
        {
            // For non-binary expressions, use ToString
            expressionString = expression.ToString();
        }

        // Remove parentheses, commas and escape special characters for Mermaid
        expressionString = expressionString
            .Replace("Convert", "", StringComparison.Ordinal)
            .Replace("(", "", StringComparison.Ordinal)
            .Replace(")", "", StringComparison.Ordinal)
            .Replace(",", "", StringComparison.Ordinal)
            .Replace("\"", "'", StringComparison.Ordinal);

        return expressionString;
    }

    private static string GetOperatorSymbol(System.Linq.Expressions.ExpressionType nodeType)
    {
        return nodeType switch
        {
            System.Linq.Expressions.ExpressionType.Equal => "==",
            System.Linq.Expressions.ExpressionType.NotEqual => "!=",
            System.Linq.Expressions.ExpressionType.GreaterThan => ">",
            System.Linq.Expressions.ExpressionType.GreaterThanOrEqual => ">=",
            System.Linq.Expressions.ExpressionType.LessThan => "<",
            System.Linq.Expressions.ExpressionType.LessThanOrEqual => "<=",
            _ => nodeType.ToString(),
        };
    }

    private static string GetConstantValue(System.Linq.Expressions.Expression expression)
    {
        // Remove Convert/Cast to get to the constant
        while (
            expression is System.Linq.Expressions.UnaryExpression unary
            && unary.NodeType == System.Linq.Expressions.ExpressionType.Convert
        )
        {
            expression = unary.Operand;
        }

        if (expression is System.Linq.Expressions.ConstantExpression constExpr)
        {
            return constExpr.Value?.ToString() ?? "null";
        }

        return expression.ToString();
    }

    private static List<INode> FindNodesThatSatisfyRequirement(
        IGraph graph,
        IRequirement<IGeneratedActor> requirement
    )
    {
        var satisfyingNodes = new List<INode>();
        var requirementExpr = NormalizeExpression(requirement.ConditionExpression.Body);

        foreach (var node in graph.Nodes)
        {
            var mutations = node.Capability.Relations?.Mutations;
            if (mutations == null)
                continue;

            foreach (var mutation in mutations)
            {
                var mutationExpr = NormalizeExpression(mutation.MutationExpression.Body);

                if (ExpressionsAreEquivalent(mutationExpr, requirementExpr))
                {
                    satisfyingNodes.Add(node);
                    break; // One mutation is enough
                }
            }
        }

        return satisfyingNodes;
    }

    private static System.Linq.Expressions.Expression NormalizeExpression(
        System.Linq.Expressions.Expression expr
    )
    {
        // Remove Convert/Cast operations
        while (
            expr is System.Linq.Expressions.UnaryExpression unary
            && unary.NodeType == System.Linq.Expressions.ExpressionType.Convert
        )
        {
            expr = unary.Operand;
        }
        return expr;
    }

    private static bool ExpressionsAreEquivalent(
        System.Linq.Expressions.Expression expr1,
        System.Linq.Expressions.Expression expr2
    )
    {
        if (expr1.NodeType != expr2.NodeType)
        {
            return false;
        }

        if (
            expr1 is System.Linq.Expressions.BinaryExpression binary1
            && expr2 is System.Linq.Expressions.BinaryExpression binary2
        )
        {
            return binary1.NodeType == binary2.NodeType
                && ExpressionsAreEquivalent(binary1.Left, binary2.Left)
                && ExpressionsAreEquivalent(binary1.Right, binary2.Right);
        }

        if (
            expr1 is System.Linq.Expressions.MemberExpression member1
            && expr2 is System.Linq.Expressions.MemberExpression member2
        )
        {
            return member1.Member.Name == member2.Member.Name;
        }

        if (
            expr1 is System.Linq.Expressions.ConstantExpression const1
            && expr2 is System.Linq.Expressions.ConstantExpression const2
        )
        {
            return Equals(const1.Value, const2.Value);
        }

        if (
            expr1 is System.Linq.Expressions.ParameterExpression
            && expr2 is System.Linq.Expressions.ParameterExpression
        )
        {
            return true;
        }

        if (
            expr1 is System.Linq.Expressions.UnaryExpression unary1
            && expr2 is System.Linq.Expressions.UnaryExpression unary2
        )
        {
            return unary1.NodeType == unary2.NodeType
                && ExpressionsAreEquivalent(unary1.Operand, unary2.Operand);
        }

        return expr1.ToString() == expr2.ToString();
    }
}

public record MermaidGraphOptions(bool UseFullName = false) { }
