using Kapa.Abstractions.Actors;
using Kapa.Abstractions.Graphs;

namespace Kapa.Core.Graphs;

public sealed class Graph : IGraph
{
    public Graph(IReadOnlyCollection<INode> nodes)
    {
        Nodes = nodes;
        MissingRequirements = new Dictionary<INode, ICollection<IEffect<IGeneratedActor>>>();
        Edges = Reduce([.. nodes], []).Edges;
    }

    private Graph(
        IReadOnlyCollection<INode> nodes,
        IDictionary<INode, ICollection<IEffect<IGeneratedActor>>> missingRequirements,
        IReadOnlyCollection<IEdge> edges
    )
    {
        Nodes = nodes;
        MissingRequirements = missingRequirements;
        Edges = edges;
    }

    /// <inheritdoc/>
    public IReadOnlyCollection<INode> Nodes { get; init; }

    /// <inheritdoc/>
    public IReadOnlyCollection<IEdge> Edges { get; init; }

    public IDictionary<
        INode,
        ICollection<IEffect<IGeneratedActor>>
    > MissingRequirements
    { get; init; }

    public IGraph Reduce(IReadOnlyList<INode> includedNodes, IReadOnlyList<INode> excludedNodes)
    {
        ArgumentNullException.ThrowIfNull(includedNodes);
        ArgumentNullException.ThrowIfNull(excludedNodes);

        // Validate that waypoints are not in excluded nodes
        var nodesInIncludedAndExcluded = includedNodes.Intersect(excludedNodes).ToList();
        if (nodesInIncludedAndExcluded.Count > 0)
        {
            var nodeNames = string.Join(
                ", ",
                nodesInIncludedAndExcluded.Select(n => $"'{n.Capability.OutcomeMetadata.Source}'")
            );

            throw new InvalidOperationException(
                $"Included node(s) cannot be in excluded nodes: {nodeNames}"
            );
        }

        var availableNodes = Nodes.Except(excludedNodes).ToHashSet();
        var requiredNodes = new HashSet<INode>();
        var visitedForCycleDetection = new HashSet<INode>();
        var currentPath = new Stack<INode>();
        var edgeMap = new Dictionary<(INode from, INode to), List<IEffect<IGeneratedActor>>>();

        // Process each included node and find its dependencies using DFS
        foreach (var includedNode in includedNodes)
        {
            if (availableNodes.Contains(includedNode))
            {
                ResolveDependencies(
                    includedNode,
                    availableNodes,
                    requiredNodes,
                    visitedForCycleDetection,
                    currentPath,
                    edgeMap
                );
            }
        }

        // Create edges with sequential indices
        var edges = new List<IEdge>();
        var edgeIndex = 1;
        foreach (var ((from, to), resolvingMutations) in edgeMap)
        {
            edges.Add(new Edge(from, to, resolvingMutations, edgeIndex));
            edgeIndex++;
        }

        return new Graph([.. requiredNodes], MissingRequirements, edges);
    }

    private void ResolveDependencies(
        INode currentNode,
        HashSet<INode> availableNodes,
        HashSet<INode> requiredNodes,
        HashSet<INode> visitedForCycleDetection,
        Stack<INode> currentPath,
        Dictionary<(INode from, INode to), List<IEffect<IGeneratedActor>>> edgeMap
    )
    {
        // Check for cycles
        if (currentPath.Contains(currentNode))
        {
            var cyclePath = string.Join(
                " -> ",
                currentPath.Reverse().Select(n => $"'{n.Capability.OutcomeMetadata.Source}'")
            );
            throw new InvalidOperationException(
                $"Circular dependency detected: {cyclePath} -> '{currentNode.Capability.OutcomeMetadata.Source}'"
            );
        }

        // Skip if already fully processed
        if (visitedForCycleDetection.Contains(currentNode))
        {
            return;
        }

        currentPath.Push(currentNode);
        requiredNodes.Add(currentNode);

        // Find all nodes that satisfy this node's requirements
        var requirements = currentNode.Capability.Relations?.Requirements;
        if (requirements is not null)
        {
            foreach (var requirement in requirements)
            {
                var satisfyingNodesWithMutations = FindNodesThatSatisfyRequirement(
                    requirement,
                    availableNodes
                );
                if (satisfyingNodesWithMutations.Count < 1)
                {
                    if (MissingRequirements.TryGetValue(currentNode, out var missingRequirements))
                    {
                        if (!missingRequirements.Any(x => x.Id == requirement.Id))
                        {
                            missingRequirements.Add(requirement);
                        }
                    }
                    else
                    {
                        MissingRequirements.Add(currentNode, [requirement]);
                    }
                }

                // Create edges and recursively process satisfying nodes
                foreach (var (satisfyingNode, mutation) in satisfyingNodesWithMutations)
                {
                    // Track edge from satisfying node to current node
                    var edgeKey = (satisfyingNode, currentNode);
                    if (!edgeMap.TryGetValue(edgeKey, out var mutations))
                    {
                        mutations = [];
                        edgeMap[edgeKey] = mutations;
                    }
                    if (!mutations.Contains(mutation))
                    {
                        mutations.Add(mutation);
                    }

                    ResolveDependencies(
                        satisfyingNode,
                        availableNodes,
                        requiredNodes,
                        visitedForCycleDetection,
                        currentPath,
                        edgeMap
                    );
                }
            }
        }

        visitedForCycleDetection.Add(currentNode);
        currentPath.Pop();
    }

    private static List<(INode node, IEffect<IGeneratedActor> mutation)> FindNodesThatSatisfyRequirement(
        IEffect<IGeneratedActor> requirement,
        HashSet<INode> availableNodes
    )
    {
        var satisfyingNodesWithMutations = new List<(INode node, IEffect<IGeneratedActor> mutation)>();

        foreach (var node in availableNodes)
        {
            var mutations = node.Capability.Relations?.Mutations;
            if (mutations is null)
                continue;

            foreach (var mutation in mutations)
            {
                // Check if the mutation's predicate matches the requirement's predicate
                if (mutation.AreEqual(requirement))
                {
                    satisfyingNodesWithMutations.Add((node, mutation));
                    break; // One mutation per node is enough
                }
            }
        }

        return satisfyingNodesWithMutations;
    }
}
