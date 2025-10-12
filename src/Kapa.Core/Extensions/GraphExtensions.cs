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
                    var satisfyingNodes = FindNodesThatSatisfyRequirement(graph, requirement);

                    foreach (var satisfyingNode in satisfyingNodes)
                    {
                        var satisfyingNodeName = GetNodeName(satisfyingNode, options);
                        var edgeKey = $"{satisfyingNodeName}-->{nodeName}";

                        if (!processedEdges.Contains(edgeKey))
                        {
                            sb.AppendLine(
                                CultureInfo.InvariantCulture,
                                $"{satisfyingNodeName} -->|{requirement.Description}| {nodeName}"
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
                            $"???[Missing] -->|{requirement.Description}| {nodeName}"
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
        // Remove parameter signature if any.
        var source = node.Capability.OutcomeMetadata.Source.Split("(").First();

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
        return source;
    }

    private static List<INode> FindNodesThatSatisfyRequirement(
        IGraph graph,
        IEffect<IGeneratedActor> requirement
    )
    {
        var satisfyingNodes = new List<INode>();

        foreach (var node in graph.Nodes)
        {
            var mutations = node.Capability.Relations?.Mutations;
            if (mutations == null)
                continue;

            foreach (var mutation in mutations)
            {
                if (mutation.AreEqual(requirement))
                {
                    satisfyingNodes.Add(node);
                }
            }
        }

        return satisfyingNodes;
    }
}

public record MermaidGraphOptions(bool UseFullName = false) { }
