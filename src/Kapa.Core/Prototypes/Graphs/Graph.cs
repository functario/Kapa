using System.Linq.Expressions;
using Kapa.Abstractions.Prototypes;

namespace Kapa.Core.Prototypes.Graphs;

public sealed class Graph<TPrototype>
    where TPrototype : IPrototype
{
    public Graph(IReadOnlyCollection<Node<TPrototype>> nodes)
    {
        Nodes = nodes;
    }

    public IReadOnlyCollection<Node<TPrototype>> Nodes { get; }

    public ICollection<Route<TPrototype>> Resolve(
        ICollection<Node<TPrototype>> orderedWaypoints,
        int maxRoutes = 50
    ) => Resolve(orderedWaypoints, [], maxRoutes);

    /// <summary>
    /// Resolve valid routes through the graph respecting mutations and requirements.
    /// </summary>
    /// <param name="orderedWaypoints">Nodes that must be visited in this order.</param>
    /// <param name="excludedNodes">Nodes to exclude from routes.</param>
    /// <param name="maxRoutes">Maximum number of routes to return.</param>
    /// <returns>Collection of valid routes.</returns>
    public ICollection<Route<TPrototype>> Resolve(
        ICollection<Node<TPrototype>> orderedWaypoints,
        ICollection<Node<TPrototype>> excludedNodes,
        int maxRoutes = 50
    )
    {
        ArgumentNullException.ThrowIfNull(orderedWaypoints);
        if (orderedWaypoints.Count == 0)
        {
            // there is no destination
            return [];
        }

        orderedWaypoints ??= [];
        excludedNodes ??= [];

        var availableNodes = Nodes.Except(excludedNodes).ToList();
        var routes = new List<Route<TPrototype>>();

        // Build all possible routes
        if (orderedWaypoints.Count > 0)
        {
            // Find routes that respect the waypoint order
            FindRoutesWithWaypoints(availableNodes, orderedWaypoints.ToList(), routes, maxRoutes);
        }
        else
        {
            // Find all valid routes without waypoint constraints
            FindAllValidRoutes(availableNodes, routes, maxRoutes);
        }

        return routes;
    }

    private static void FindRoutesWithWaypoints(
        List<Node<TPrototype>> availableNodes,
        List<Node<TPrototype>> waypoints,
        List<Route<TPrototype>> routes,
        int maxRoutes
    )
    {
        if (routes.Count >= maxRoutes)
            return;

        // For each waypoint, try to build a path that satisfies its requirements
        var path = new List<Edge<TPrototype>>();
        var visitedNodes = new HashSet<Node<TPrototype>>();
        var currentPath = new HashSet<Node<TPrototype>>();

        foreach (var waypoint in waypoints)
        {
            if (!TryResolveNode(waypoint, availableNodes, visitedNodes, currentPath, path))
            {
                // Cannot satisfy this waypoint, skip this route
                return;
            }
            visitedNodes.Add(waypoint);
        }

        if (path.Count > 0)
        {
            routes.Add(new Route<TPrototype>(path));
        }
    }

    private static void FindAllValidRoutes(
        List<Node<TPrototype>> availableNodes,
        List<Route<TPrototype>> routes,
        int maxRoutes
    )
    {
        // Find routes for nodes that have requirements
        var nodesWithRequirements = availableNodes.Where(n => n.Requirements.Count > 0).ToList();

        foreach (var targetNode in nodesWithRequirements)
        {
            if (routes.Count >= maxRoutes)
                break;

            var path = new List<Edge<TPrototype>>();
            var visitedNodes = new HashSet<Node<TPrototype>>();
            var currentPath = new HashSet<Node<TPrototype>>();

            if (TryResolveNode(targetNode, availableNodes, visitedNodes, currentPath, path))
            {
                visitedNodes.Add(targetNode);
                routes.Add(new Route<TPrototype>(path));
            }
        }
    }

    private static bool TryResolveNode(
        Node<TPrototype> targetNode,
        List<Node<TPrototype>> availableNodes,
        HashSet<Node<TPrototype>> visitedNodes,
        HashSet<Node<TPrototype>> currentPath,
        List<Edge<TPrototype>> path
    )
    {
        // Detect circular dependency
        if (currentPath.Contains(targetNode))
            return false; // Cycle detected, abort this path

        if (targetNode.Requirements.Count == 0)
            return true;

        // Mark this node as being visited in the current recursion path
        currentPath.Add(targetNode);

        try
        {
            // Find nodes that can satisfy this node's requirements
            foreach (var requirement in targetNode.Requirements)
            {
                var satisfyingNodes = FindNodesSatisfyingRequirement(
                    requirement,
                    availableNodes,
                    visitedNodes
                );

                if (satisfyingNodes.Count == 0)
                    return false; // Cannot satisfy this requirement

                // For now, take the first satisfying node (can be extended for multiple paths)
                var (providerNode, mutations) = satisfyingNodes.First();

                // Recursively resolve the provider node's requirements
                if (!visitedNodes.Contains(providerNode))
                {
                    if (
                        !TryResolveNode(
                            providerNode,
                            availableNodes,
                            visitedNodes,
                            currentPath,
                            path
                        )
                    )
                        return false;

                    visitedNodes.Add(providerNode);
                }

                // Add edge from provider to target
                path.Add(new Edge<TPrototype>(providerNode, targetNode, mutations));
            }

            return true;
        }
        finally
        {
            // Remove from current path when backtracking
            currentPath.Remove(targetNode);
        }
    }

    private static List<(
        Node<TPrototype> Node,
        List<IMutation<TPrototype>> Mutations
    )> FindNodesSatisfyingRequirement(
        IRequirement<TPrototype> requirement,
        List<Node<TPrototype>> availableNodes,
        HashSet<Node<TPrototype>> visitedNodes
    )
    {
        var results = new List<(Node<TPrototype>, List<IMutation<TPrototype>>)>();

        foreach (var node in availableNodes)
        {
            if (visitedNodes.Contains(node))
                continue;

            var satisfyingMutations = new List<IMutation<TPrototype>>();

            foreach (var mutation in node.Mutations)
            {
                if (CanMutationSatisfyRequirement(mutation, requirement))
                {
                    satisfyingMutations.Add(mutation);
                }
            }

            if (satisfyingMutations.Count > 0)
            {
                results.Add((node, satisfyingMutations));
            }
        }

        return results;
    }

    private static bool CanMutationSatisfyRequirement(
        IMutation<TPrototype> mutation,
        IRequirement<TPrototype> requirement
    )
    {
        // Extract properties from both mutation and requirement expressions
        var mutationProps = ExtractPropertyNames(mutation.MutationExpression);
        return requirement.ReferencedProperties.Overlaps(mutationProps);
    }

    private static HashSet<string> ExtractPropertyNames(Expression expression)
    {
        var visitor = new PropertyVisitor();
        visitor.Visit(expression);
        return visitor.Properties;
    }

    private sealed class PropertyVisitor : ExpressionVisitor
    {
        public HashSet<string> Properties { get; } = [];

        protected override Expression VisitMember(MemberExpression node)
        {
            // Only add properties from the declaring type (not nested types)
            if (
                node.Member.DeclaringType != null
                && node.Expression?.NodeType == ExpressionType.Parameter
            )
            {
                Properties.Add(node.Member.Name);
            }
            return base.VisitMember(node);
        }
    }
}
