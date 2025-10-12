using Kapa.Abstractions.Actors;
using Kapa.Abstractions.Graphs;
using Kapa.Core.Extensions;

namespace Kapa.Core.Graphs;

public sealed class Graph : IGraph
{
    public Graph(IReadOnlyCollection<INode> nodes)
    {
        Nodes = nodes;
    }

    public IReadOnlyCollection<INode> Nodes { get; }

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
                    currentPath
                );
            }
        }

        return new Graph([.. requiredNodes]);
    }

    private static void ResolveDependencies(
        INode currentNode,
        HashSet<INode> availableNodes,
        HashSet<INode> requiredNodes,
        HashSet<INode> visitedForCycleDetection,
        Stack<INode> currentPath
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
        if (requirements != null)
        {
            foreach (var requirement in requirements)
            {
                var satisfyingNodes = FindNodesThatSatisfyRequirement(requirement, availableNodes);

                // Recursively process satisfying nodes
                foreach (var satisfyingNode in satisfyingNodes)
                {
                    ResolveDependencies(
                        satisfyingNode,
                        availableNodes,
                        requiredNodes,
                        visitedForCycleDetection,
                        currentPath
                    );
                }
            }
        }

        visitedForCycleDetection.Add(currentNode);
        currentPath.Pop();
    }

    private static List<INode> FindNodesThatSatisfyRequirement(
        IEffect<IGeneratedActor> requirement,
        HashSet<INode> availableNodes
    )
    {
        var satisfyingNodes = new List<INode>();

        foreach (var node in availableNodes)
        {
            var mutations = node.Capability.Relations?.Mutations;
            if (mutations == null)
                continue;

            foreach (var mutation in mutations)
            {
                // Check if the mutation's predicate matches the requirement's predicate
                // We need to compare the underlying delegates, not the wrapper
                if (mutation.Predicate.ArePredicatesEquivalent(requirement.Predicate))
                {
                    satisfyingNodes.Add(node);
                    break; // One mutation is enough
                }
            }
        }

        return satisfyingNodes;
    }
}
