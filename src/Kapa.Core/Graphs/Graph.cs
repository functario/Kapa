using System.Linq.Expressions;
using Kapa.Abstractions.Graphs;
using Kapa.Abstractions.Prototypes;

namespace Kapa.Core.Graphs;

public sealed class Graph : IGraph
{
    public Graph(IReadOnlyCollection<INode> nodes)
    {
        Nodes = nodes;
    }

    public IReadOnlyCollection<INode> Nodes { get; }

    public IGraph FocusGraph(ICollection<INode> orderedWaypoints, ICollection<INode> excludedNodes)
    {
        ArgumentNullException.ThrowIfNull(orderedWaypoints);
        ArgumentNullException.ThrowIfNull(excludedNodes);

        // Validate that waypoints are not in excluded nodes
        var waypointsInExcluded = orderedWaypoints.Intersect(excludedNodes).ToList();
        if (waypointsInExcluded.Count > 0)
        {
            var nodeNames = string.Join(
                ", ",
                waypointsInExcluded.Select(n => $"'{n.Capability.OutcomeMetadata.Source}'")
            );
            throw new InvalidOperationException(
                $"Waypoint(s) cannot be in excluded nodes: {nodeNames}"
            );
        }

        var availableNodes = Nodes.Except(excludedNodes).ToHashSet();
        var focusedNodes = new HashSet<INode>(orderedWaypoints);
        var resolutionStack = new Stack<INode>();

        // Process each waypoint to find dependencies
        foreach (var waypoint in orderedWaypoints)
        {
            ResolveDependencies(waypoint, availableNodes, focusedNodes, resolutionStack);
        }

        return new Graph([.. focusedNodes]);
    }

    private static void ResolveDependencies(
        INode node,
        HashSet<INode> availableNodes,
        HashSet<INode> focusedNodes,
        Stack<INode> resolutionStack
    )
    {
        // Check for circular dependency
        if (resolutionStack.Contains(node))
        {
            var cycle = string.Join(
                " → ",
                resolutionStack
                    .Reverse()
                    .Select(n => n.Capability.OutcomeMetadata.Source)
                    .Concat([node.Capability.OutcomeMetadata.Source])
            );
            throw new InvalidOperationException($"Circular dependency detected: {cycle}");
        }

        // If no requirements, nothing to resolve
        if (node.Requirements.Count == 0)
        {
            return;
        }

        resolutionStack.Push(node);

        try
        {
            // Parse all requirements and extract atomic conditions
            var atomicConditions = new List<AtomicCondition>();
            foreach (var requirement in node.Requirements)
            {
                var conditions = ExtractAtomicConditions(requirement.ConditionExpression);
                atomicConditions.AddRange(conditions);
            }

            // For each condition, find nodes that can satisfy it
            foreach (var condition in atomicConditions)
            {
                var satisfyingNodes = FindNodesThatSatisfyCondition(condition, availableNodes);

                if (satisfyingNodes.Count == 0)
                {
                    throw new InvalidOperationException(
                        $"Cannot satisfy requirement for node '{node.Capability.OutcomeMetadata.Source}'. "
                            + $"No capability provides mutation for condition: {condition.Expression}"
                    );
                }

                // Add all satisfying nodes to the focused graph
                foreach (var satisfyingNode in satisfyingNodes)
                {
                    if (focusedNodes.Add(satisfyingNode))
                    {
                        // Recursively resolve this node's dependencies
                        ResolveDependencies(
                            satisfyingNode,
                            availableNodes,
                            focusedNodes,
                            resolutionStack
                        );
                    }
                }
            }
        }
        finally
        {
            resolutionStack.Pop();
        }
    }

    private static List<AtomicCondition> ExtractAtomicConditions(LambdaExpression expression)
    {
        var conditions = new List<AtomicCondition>();

        // Since requirements must be simple (no &&, ||), each requirement is a single atomic condition
        var body = NormalizeExpression(expression.Body);
        conditions.Add(new AtomicCondition(body));

        return conditions;
    }

    private static List<INode> FindNodesThatSatisfyCondition(
        AtomicCondition condition,
        HashSet<INode> availableNodes
    )
    {
        var satisfyingNodes = new List<INode>();

        foreach (var node in availableNodes)
        {
            foreach (var mutation in node.Mutations)
            {
                if (DoesMutationSatisfyCondition(mutation, condition))
                {
                    satisfyingNodes.Add(node);
                    break; // One mutation is enough, move to next node
                }
            }
        }

        return satisfyingNodes;
    }

    private static bool DoesMutationSatisfyCondition(
        IMutation<IGeneratedPrototype> mutation,
        AtomicCondition condition
    )
    {
        // Validate mutation is simple
        ValidateMutationIsSimple(mutation.MutationExpression);

        var mutationExpr = mutation.MutationExpression.Body;
        var conditionExpr = condition.Expression;

        // Normalize both expressions
        mutationExpr = NormalizeExpression(mutationExpr);
        conditionExpr = NormalizeExpression(conditionExpr);

        // Compare expressions for equivalence
        return ExpressionsAreEquivalent(mutationExpr, conditionExpr);
    }

    private static void ValidateMutationIsSimple(LambdaExpression expression)
    {
        var body = NormalizeExpression(expression.Body);

        // Check for logical operators
        if (body is BinaryExpression binary)
        {
            if (
                binary.NodeType == ExpressionType.AndAlso
                || binary.NodeType == ExpressionType.OrElse
            )
            {
                throw new InvalidOperationException(
                    $"Mutation expressions must be simple (single comparison without &&, ||). "
                        + $"Found: {expression}"
                );
            }
        }
    }

    private static Expression NormalizeExpression(Expression expr)
    {
        // Remove Convert/Cast operations
        while (expr is UnaryExpression unary && unary.NodeType == ExpressionType.Convert)
        {
            expr = unary.Operand;
        }
        return expr;
    }

    private static bool ExpressionsAreEquivalent(Expression expr1, Expression expr2)
    {
        if (expr1.NodeType != expr2.NodeType)
        {
            return false;
        }

        if (expr1 is BinaryExpression binary1 && expr2 is BinaryExpression binary2)
        {
            return binary1.NodeType == binary2.NodeType
                && ExpressionsAreEquivalent(binary1.Left, binary2.Left)
                && ExpressionsAreEquivalent(binary1.Right, binary2.Right);
        }

        if (expr1 is MemberExpression member1 && expr2 is MemberExpression member2)
        {
            return member1.Member.Name == member2.Member.Name;
        }

        if (expr1 is ConstantExpression const1 && expr2 is ConstantExpression const2)
        {
            return Equals(const1.Value, const2.Value);
        }

        if (expr1 is ParameterExpression && expr2 is ParameterExpression)
        {
            return true; // Parameters are equivalent in our context
        }

        if (expr1 is UnaryExpression unary1 && expr2 is UnaryExpression unary2)
        {
            return unary1.NodeType == unary2.NodeType
                && ExpressionsAreEquivalent(unary1.Operand, unary2.Operand);
        }

        // Fallback to string comparison
        return expr1.ToString() == expr2.ToString();
    }

    private sealed class AtomicCondition
    {
        public Expression Expression { get; }

        public AtomicCondition(Expression expression)
        {
            Expression = expression;
        }
    }

    public ICollection<IRoute> Resolve(ICollection<INode> orderedWaypoints, int maxRoutes = 50) =>
        Resolve(orderedWaypoints, [], maxRoutes);

    /// <summary>
    /// Resolve valid routes through the graph respecting mutations and requirements.
    /// </summary>
    /// <param name="orderedWaypoints">Nodes that must be visited in this order.</param>
    /// <param name="excludedNodes">Nodes to exclude from routes.</param>
    /// <param name="maxRoutes">Maximum number of routes to return.</param>
    /// <returns>Collection of valid routes.</returns>
#pragma warning disable CA1822 // Mark members as static
    public ICollection<IRoute> Resolve(
#pragma warning restore CA1822 // Mark members as static
        ICollection<INode> orderedWaypoints,
        ICollection<INode> excludedNodes,
#pragma warning disable IDE0060 // Remove unused parameter
        int maxRoutes = 50
#pragma warning restore IDE0060 // Remove unused parameter
    )
    {
        ArgumentNullException.ThrowIfNull(orderedWaypoints);
        ArgumentNullException.ThrowIfNull(excludedNodes);
        throw new NotImplementedException();
    }
}
