using System.Globalization;
using System.Text;
using Kapa.Abstractions.Actors;
using Kapa.Abstractions.Graphs;
using Kapa.Core.Graphs;
using Kapa.Core.Mermaids;

namespace Kapa.Core.Extensions;

public static class GraphExtensions
{
    public static string ToMermaidGraph(this IGraph graph, MermaidGraphOptions? options = null)
    {
        ArgumentNullException.ThrowIfNull(graph);
        options ??= new MermaidGraphOptions();

        var sb = new StringBuilder();
        sb.AppendLine(
            """
            ---
            config:
              layout: elk
            ---
            """
        );

        sb.AppendLine(
            CultureInfo.InvariantCulture,
            $"graph {Enum.GetName(options.GraphOrientations)}"
        );

        // Build edge reference tracking from graph.Edges
        var edgeRefs = new Dictionary<(string nodeName, string reqId), List<int>>();
        
        foreach (var edge in graph.Edges)
        {
            var toNodeName = GetNodeName(edge.ToCapacity, options);
            
            // Map each resolving mutation to the requirements it satisfies
            foreach (var mutation in edge.ResolvingMutations)
            {
                // Find matching requirements in the target node
                var requirements = edge.ToCapacity.Capability.Relations?.Requirements;
                if (requirements != null)
                {
                    foreach (var requirement in requirements)
                    {
                        if (mutation.AreEqual(requirement))
                        {
                            var key = (toNodeName, requirement.Id);
                            if (!edgeRefs.TryGetValue(key, out var list))
                            {
                                list = [];
                                edgeRefs[key] = list;
                            }
                            list.Add(edge.Index);
                            break; // One mutation matches one requirement
                        }
                    }
                }
            }
        }

        // Render all nodes with requirements in the box, including edge refs based on option
        foreach (var node in graph.Nodes)
        {
            var nodeName = GetNodeName(node, options);
            var description = options.DisplayDescription
                ? $"""
                    <br/>'{node.Capability.Description}' 
                    """
                : "";

            var relations = node.Capability.Relations;
            var requirements = relations?.Requirements ?? [];
            var missing =
                graph is Graph g && g.MissingRequirements.TryGetValue(node, out var missingReqs)
                    ? missingReqs
                    : [];

            var requirementsText = string.Empty;
            if (options.DisplayNodeRequirementOptions != DisplayNodeRequirementOptions.None)
            {
                IEnumerable<IEffect<IGeneratedActor>> reqsToShow = requirements;
                if (options.DisplayOnlyNodeMissingRequirements)
                {
                    reqsToShow = missing;
                }

                if (reqsToShow.Any())
                {
                    var lines = new List<string>();
                    foreach (var req in reqsToShow)
                    {
                        var isMissing = missing.Any(m => m.Id == req.Id);
                        var marker = isMissing ? "❌" : "✅";
                        var key = (nodeName, req.Id);
                        var refs = "";

                        if (
                            options.DisplayNodeRequirementOptions
                                != DisplayNodeRequirementOptions.None
                            && !isMissing
                        )
                        {
                            if (
                                options.DisplayNodeRequirementOptions
                                == DisplayNodeRequirementOptions.ReferenceDirectEdges
                            )
                            {
                                // Show only direct edges that resolve this requirement
                                if (edgeRefs.TryGetValue(key, out var refList) && refList.Count > 0)
                                {
                                    refs = " [" + string.Join(", ", refList) + "]";
                                }
                            }
                            else if (
                                options.DisplayNodeRequirementOptions
                                == DisplayNodeRequirementOptions.ReferenceInheritedEdges
                            )
                            {
                                // Show all edges (direct and recursive) that resolve this requirement
                                var allRefs = CollectRecursiveEdgeReferences(
                                    node,
                                    req,
                                    graph,
                                    edgeRefs,
                                    options
                                );
                                if (allRefs.Count > 0)
                                {
                                    refs = " [" + string.Join(", ", allRefs) + "]";
                                }
                            }
                        }

                        lines.Add($"{marker} {req.Description}{refs}");
                    }
                    requirementsText = "<br/>" + string.Join("<br/>", lines);
                }
            }

            sb.AppendLine(nodeName + "[\"" + nodeName + description + requirementsText + "\"]");
        }

        // Group edges by (from, to) pair for merged rendering
        var mergedEdges = new Dictionary<(string from, string to), List<(IEffect<IGeneratedActor> mutation, int index)>>();
        
        foreach (var edge in graph.Edges)
        {
            var fromName = GetNodeName(edge.FromCapacity, options);
            var toName = GetNodeName(edge.ToCapacity, options);
            var key = (fromName, toName);
            
            if (!mergedEdges.TryGetValue(key, out var mutations))
            {
                mutations = [];
                mergedEdges[key] = mutations;
            }
            
            foreach (var mutation in edge.ResolvingMutations)
            {
                mutations.Add((mutation, edge.Index));
            }
        }

        // Render merged edges
        foreach (var ((from, to), mutations) in mergedEdges)
        {
            if (options.DisplayNodeRequirementOptions != DisplayNodeRequirementOptions.None)
            {
                var combinedLabel = string.Join(
                    "<br/>",
                    mutations.Select(m => $"{m.mutation.Description} [{m.index}]")
                );
                sb.AppendLine(($"{from} -->|\"{combinedLabel}\"| {to}").ToString());
            }
            else
            {
                var combinedLabel = string.Join("<br/>", mutations.Select(m => m.mutation.Description));
                sb.AppendLine(($"{from} -->|\"{combinedLabel}\"| {to}").ToString());
            }
        }

        return sb.ToString();
    }

    private static string GetNodeName(INode node, MermaidGraphOptions options)
    {
        // Remove parameter signature if any.
        var source = node.Capability.OutcomeMetadata.Source.Split("(").First();

        // Extract just the capability name if not using full name
        if (!options.ReplaceNameBySource)
        {
            return node.Capability.Name;
        }

        // Remove only parentheses and angle brackets (dots are allowed in Mermaid)
        return source;
    }

    private static List<int> CollectRecursiveEdgeReferences(
        INode node,
        IEffect<IGeneratedActor> requirement,
        IGraph graph,
        Dictionary<(string nodeName, string reqId), List<int>> edgeRefs,
        MermaidGraphOptions options
    )
    {
        var allRefs = new HashSet<int>();
        var visited = new HashSet<string>();
        var nodeName = GetNodeName(node, options);

        // Collect all edges that resolve this requirement type in the entire dependency chain
        CollectRecursiveEdgeReferencesForRequirement(
            nodeName,
            requirement.Id,
            graph,
            edgeRefs,
            options,
            allRefs,
            visited
        );

        return [.. allRefs.OrderBy(x => x)];
    }

    private static void CollectRecursiveEdgeReferencesForRequirement(
        string currentNodeName,
        string requirementId,
        IGraph graph,
        Dictionary<(string nodeName, string reqId), List<int>> edgeRefs,
        MermaidGraphOptions options,
        HashSet<int> allRefs,
        HashSet<string> visited
    )
    {
        // Avoid cycles
        if (visited.Contains(currentNodeName))
            return;

        visited.Add(currentNodeName);

        var currentNode = graph.Nodes.FirstOrDefault(n =>
            GetNodeName(n, options) == currentNodeName
        );
        if (currentNode == null)
            return;

        // First, check if this node has the requirement we're looking for
        // and collect all edges that resolve it
        var key = (currentNodeName, requirementId);
        if (edgeRefs.TryGetValue(key, out var directRefs))
        {
            foreach (var directRef in directRefs)
            {
                allRefs.Add(directRef);
            }
        }

        // Then, recursively traverse all dependencies of this node
        // to find if any of them also have the same requirement resolved
        var requirements = currentNode.Capability.Relations?.Requirements;
        if (requirements != null)
        {
            foreach (var req in requirements)
            {
                // Find all edges that point to this node for this requirement
                var reqKey = (currentNodeName, req.Id);
                if (edgeRefs.TryGetValue(reqKey, out var reqRefs))
                {
                    foreach (var reqRef in reqRefs)
                    {
                        // Find the edge with this index
                        var edge = graph.Edges.FirstOrDefault(e => e.Index == reqRef);
                        if (edge != null)
                        {
                            var fromName = GetNodeName(edge.FromCapacity, options);
                            // Recursively check the source node for the same requirement type
                            CollectRecursiveEdgeReferencesForRequirement(
                                fromName,
                                requirementId, // Look for the SAME requirement type
                                graph,
                                edgeRefs,
                                options,
                                allRefs,
                                visited
                            );
                        }
                    }
                }
            }
        }
    }
}
