using System.Linq.Expressions;
using Kapa.Abstractions.Prototypes;

namespace Kapa.Core.Prototypes.Graphs;

public sealed class Graph
{
    public Graph(IReadOnlyCollection<Node> nodes)
    {
        Nodes = nodes;
    }

    public IReadOnlyCollection<Node> Nodes { get; }

    public ICollection<Route> Resolve(ICollection<Node> orderedWaypoints, int maxRoutes = 50) =>
        Resolve(orderedWaypoints, [], maxRoutes);

    /// <summary>
    /// Resolve valid routes through the graph respecting mutations and requirements.
    /// </summary>
    /// <param name="orderedWaypoints">Nodes that must be visited in this order.</param>
    /// <param name="excludedNodes">Nodes to exclude from routes.</param>
    /// <param name="maxRoutes">Maximum number of routes to return.</param>
    /// <returns>Collection of valid routes.</returns>
    public ICollection<Route> Resolve(
        ICollection<Node> orderedWaypoints,
        ICollection<Node> excludedNodes,
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
        var routes = new List<Route>();

        // Build all possible routes
        if (orderedWaypoints.Count > 0)
        {
            // Find routes that respect the waypoint order
            FindRoutesWithWaypoints(availableNodes, [.. orderedWaypoints], routes, maxRoutes);
        }
        else
        {
            // Find all valid routes without waypoint constraints
            FindAllValidRoutes(availableNodes, routes, maxRoutes);
        }

        return routes;
    }

    private static void FindRoutesWithWaypoints(
        List<Node> availableNodes,
        List<Node> waypoints,
        List<Route> routes,
        int maxRoutes
    )
    {
        if (routes.Count >= maxRoutes)
            return;

        // For each waypoint, find all possible orderings of dependencies
        foreach (var waypoint in waypoints)
        {
            if (waypoint.Requirements.Count == 0)
            {
                // No dependencies, just add empty route
                continue;
            }

            // Find all nodes that contribute to satisfying this waypoint's requirements
            var contributingNodes = new List<Node>();
            var allRequiredProps = new HashSet<string>();

            foreach (var req in waypoint.Requirements)
            {
                allRequiredProps.UnionWith(req.ReferencedProperties);
            }

            foreach (var node in availableNodes)
            {
                if (node == waypoint)
                    continue;

                foreach (var mutation in node.Mutations)
                {
                    var mutationProps = ExtractPropertyNames(mutation.MutationExpression);
                    if (allRequiredProps.Overlaps(mutationProps))
                    {
                        if (!contributingNodes.Contains(node))
                        {
                            contributingNodes.Add(node);
                        }
                    }
                }
            }

            if (contributingNodes.Count == 0)
                continue;

            // Generate all permutations of contributing nodes
            var permutations = GetPermutations(contributingNodes);

            foreach (var ordering in permutations)
            {
                if (routes.Count >= maxRoutes)
                    break;

                // Check if this ordering satisfies all requirements
                var satisfiedProps = new HashSet<string>();
                var edges = new List<Edge>();

                foreach (var providerNode in ordering)
                {
                    var contributedMutations = new List<IMutation<IPrototype>>();

                    foreach (var mutation in providerNode.Mutations)
                    {
                        var mutationProps = ExtractPropertyNames(mutation.MutationExpression);
                        if (allRequiredProps.Overlaps(mutationProps))
                        {
                            contributedMutations.Add(mutation);
                            satisfiedProps.UnionWith(mutationProps);
                        }
                    }

                    if (contributedMutations.Count > 0)
                    {
                        edges.Add(new Edge(providerNode, waypoint, contributedMutations));
                    }
                }

                // Check if all required properties are satisfied
                if (allRequiredProps.IsSubsetOf(satisfiedProps))
                {
                    routes.Add(new Route(edges));
                }
            }
        }
    }

    private static List<List<T>> GetPermutations<T>(List<T> list)
    {
        var result = new List<List<T>>();
        if (list.Count == 0)
        {
            result.Add(new List<T>());
            return result;
        }

        for (int i = 0; i < list.Count; i++)
        {
            var element = list[i];
            var remaining = list.Where((_, index) => index != i).ToList();
            var subPermutations = GetPermutations(remaining);

            foreach (var subPerm in subPermutations)
            {
                var perm = new List<T> { element };
                perm.AddRange(subPerm);
                result.Add(perm);
            }
        }

        return result;
    }

    private static void FindAllRoutesToNode(
        Node targetNode,
        List<Node> availableNodes,
        HashSet<Node> visitedNodes,
        HashSet<Node> currentPath,
        List<Edge> edgePath,
        List<List<Edge>> allPaths,
        int maxRoutes
    )
    {
        if (allPaths.Count >= maxRoutes)
            return;

        // Detect circular dependency
        if (currentPath.Contains(targetNode))
            return;

        if (targetNode.Requirements.Count == 0)
        {
            // Leaf node - no requirements, add current path
            if (edgePath.Count > 0)
            {
                allPaths.Add(new List<Edge>(edgePath));
            }
            return;
        }

        // Mark this node as being visited in the current recursion path
        currentPath.Add(targetNode);

        try
        {
            // For each requirement, find all combinations of nodes that can satisfy it
            var requirementSatisfiers = new List<List<(Node, List<IMutation<IPrototype>>)>>();

            foreach (var requirement in targetNode.Requirements)
            {
                var satisfyingNodes = FindNodesSatisfyingRequirement(
                    requirement,
                    availableNodes,
                    visitedNodes
                );

                if (satisfyingNodes.Count == 0)
                    return; // Cannot satisfy this requirement

                requirementSatisfiers.Add(satisfyingNodes);
            }

            // Generate all permutations of provider nodes across all requirements
            GenerateAllPermutations(
                targetNode,
                requirementSatisfiers,
                0,
                new List<(Node, List<IMutation<IPrototype>>)>(),
                availableNodes,
                visitedNodes,
                currentPath,
                edgePath,
                allPaths,
                maxRoutes
            );
        }
        finally
        {
            currentPath.Remove(targetNode);
        }
    }

    private static void GenerateAllPermutations(
        Node targetNode,
        List<List<(Node, List<IMutation<IPrototype>>)>> requirementSatisfiers,
        int requirementIndex,
        List<(Node, List<IMutation<IPrototype>>)> currentCombination,
        List<Node> availableNodes,
        HashSet<Node> visitedNodes,
        HashSet<Node> currentPath,
        List<Edge> edgePath,
        List<List<Edge>> allPaths,
        int maxRoutes
    )
    {
        if (allPaths.Count >= maxRoutes)
            return;

        if (requirementIndex >= requirementSatisfiers.Count)
        {
            // We have a complete combination, now try all orderings
            var providerNodes = currentCombination.Select(x => x.Item1).Distinct().ToList();
            GenerateAllOrderings(
                targetNode,
                providerNodes,
                currentCombination,
                0,
                new List<Node>(),
                availableNodes,
                new HashSet<Node>(visitedNodes),
                currentPath,
                new List<Edge>(edgePath),
                allPaths,
                maxRoutes
            );
            return;
        }

        // Try each satisfier for the current requirement
        foreach (var satisfier in requirementSatisfiers[requirementIndex])
        {
            currentCombination.Add(satisfier);
            GenerateAllPermutations(
                targetNode,
                requirementSatisfiers,
                requirementIndex + 1,
                currentCombination,
                availableNodes,
                visitedNodes,
                currentPath,
                edgePath,
                allPaths,
                maxRoutes
            );
            currentCombination.RemoveAt(currentCombination.Count - 1);

            if (allPaths.Count >= maxRoutes)
                break;
        }
    }

    private static void GenerateAllOrderings(
        Node targetNode,
        List<Node> providerNodes,
        List<(Node, List<IMutation<IPrototype>>)> mutations,
        int index,
        List<Node> currentOrder,
        List<Node> availableNodes,
        HashSet<Node> visitedNodes,
        HashSet<Node> currentPath,
        List<Edge> edgePath,
        List<List<Edge>> allPaths,
        int maxRoutes
    )
    {
        if (allPaths.Count >= maxRoutes)
            return;

        if (index >= providerNodes.Count)
        {
            // Complete ordering found, resolve in this order
            var pathCopy = new List<Edge>(edgePath);
            var visitedCopy = new HashSet<Node>(visitedNodes);

            foreach (var providerNode in currentOrder)
            {
                if (!visitedCopy.Contains(providerNode))
                {
                    // Recursively resolve this provider
                    var subPath = new List<Edge>();
                    var subVisited = new HashSet<Node>(visitedCopy);
                    var subCurrentPath = new HashSet<Node>(currentPath);

                    if (TryResolveNodeLinear(providerNode, availableNodes, subVisited, subCurrentPath, subPath))
                    {
                        pathCopy.AddRange(subPath);
                        visitedCopy.UnionWith(subVisited);
                    }
                    else
                    {
                        return; // This ordering doesn't work
                    }

                    visitedCopy.Add(providerNode);
                }

                // Add edges from provider to target
                var relevantMutations = mutations
                    .Where(m => m.Item1 == providerNode)
                    .SelectMany(m => m.Item2)
                    .ToList();

                if (relevantMutations.Count > 0)
                {
                    pathCopy.Add(new Edge(providerNode, targetNode, relevantMutations));
                }
            }

            allPaths.Add(pathCopy);
            return;
        }

        // Try each remaining provider in the next position
        for (int i = 0; i < providerNodes.Count; i++)
        {
            if (!currentOrder.Contains(providerNodes[i]))
            {
                currentOrder.Add(providerNodes[i]);
                GenerateAllOrderings(
                    targetNode,
                    providerNodes,
                    mutations,
                    index + 1,
                    currentOrder,
                    availableNodes,
                    visitedNodes,
                    currentPath,
                    edgePath,
                    allPaths,
                    maxRoutes
                );
                currentOrder.RemoveAt(currentOrder.Count - 1);

                if (allPaths.Count >= maxRoutes)
                    break;
            }
        }
    }

    private static bool TryResolveNodeLinear(
        Node targetNode,
        List<Node> availableNodes,
        HashSet<Node> visitedNodes,
        HashSet<Node> currentPath,
        List<Edge> path
    )
    {
        // Detect circular dependency
        if (currentPath.Contains(targetNode))
            return false;

        if (targetNode.Requirements.Count == 0)
            return true;

        currentPath.Add(targetNode);

        try
        {
            foreach (var requirement in targetNode.Requirements)
            {
                var satisfyingNodes = FindNodesSatisfyingRequirement(
                    requirement,
                    availableNodes,
                    visitedNodes
                );

                if (satisfyingNodes.Count == 0)
                    return false;

                var (providerNode, mutations) = satisfyingNodes.First();

                if (!visitedNodes.Contains(providerNode))
                {
                    if (!TryResolveNodeLinear(providerNode, availableNodes, visitedNodes, currentPath, path))
                        return false;

                    visitedNodes.Add(providerNode);
                }

                path.Add(new Edge(providerNode, targetNode, mutations));
            }

            return true;
        }
        finally
        {
            currentPath.Remove(targetNode);
        }
    }

    private static void FindAllValidRoutes(
        List<Node> availableNodes,
        List<Route> routes,
        int maxRoutes
    )
    {
        // Find routes for nodes that have requirements
        var nodesWithRequirements = availableNodes.Where(n => n.Requirements.Count > 0).ToList();

        foreach (var targetNode in nodesWithRequirements)
        {
            if (routes.Count >= maxRoutes)
                break;

            var path = new List<Edge>();
            var visitedNodes = new HashSet<Node>();
            var currentPath = new HashSet<Node>();

            if (TryResolveNode(targetNode, availableNodes, visitedNodes, currentPath, path))
            {
                visitedNodes.Add(targetNode);
                routes.Add(new Route(path));
            }
        }
    }

    private static bool TryResolveNode(
        Node targetNode,
        List<Node> availableNodes,
        HashSet<Node> visitedNodes,
        HashSet<Node> currentPath,
        List<Edge> path
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

                // Check if we can collectively satisfy all referenced properties
                var requiredProperties = requirement.ReferencedProperties;
                var satisfiedProperties = new HashSet<string>();
                var selectedNodes = new List<(Node, List<IMutation<IPrototype>>)>();

                // Collect all nodes that contribute to satisfying the requirement
                foreach (var (providerNode, mutations) in satisfyingNodes)
                {
                    var contributedProps = new HashSet<string>();
                    foreach (var mutation in mutations)
                    {
                        var mutationProps = ExtractPropertyNames(mutation.MutationExpression);
                        contributedProps.UnionWith(mutationProps);
                    }

                    // Only include nodes that contribute new properties
                    if (contributedProps.Except(satisfiedProperties).Any())
                    {
                        selectedNodes.Add((providerNode, mutations));
                        satisfiedProperties.UnionWith(contributedProps);

                        // If we've satisfied all properties, we can stop
                        if (requiredProperties.IsSubsetOf(satisfiedProperties))
                            break;
                    }
                }

                // Check if all required properties are satisfied
                if (!requiredProperties.IsSubsetOf(satisfiedProperties))
                    return false; // Cannot fully satisfy this requirement

                // Recursively resolve each provider node and add edges
                foreach (var (providerNode, mutations) in selectedNodes)
                {
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
                    path.Add(new Edge(providerNode, targetNode, mutations));
                }
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
        Node Node,
        List<IMutation<IPrototype>> Mutations
    )> FindNodesSatisfyingRequirement(
        IRequirement<IPrototype> requirement,
        List<Node> availableNodes,
        HashSet<Node> visitedNodes
    )
    {
        var results = new List<(Node, List<IMutation<IPrototype>>)>();

        foreach (var node in availableNodes)
        {
            if (visitedNodes.Contains(node))
                continue;

            var satisfyingMutations = new List<IMutation<IPrototype>>();

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
        IMutation<IPrototype> mutation,
        IRequirement<IPrototype> requirement
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
