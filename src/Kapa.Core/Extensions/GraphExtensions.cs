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

        // Track edges and assign reference numbers
        var edgeList = new List<(string from, string to, string label, string reqId)>();
        var edgeCounter = 1;
        var edgeRefs = new Dictionary<(string nodeName, string reqId), List<int>>();

        // First pass: collect edges and assign numbers
        foreach (var node in graph.Nodes)
        {
            var nodeName = GetNodeName(node, options);
            var relations = node.Capability.Relations;
            if (relations?.Requirements != null && relations.Requirements.Count > 0)
            {
                foreach (var requirement in relations.Requirements)
                {
                    foreach (var otherNode in graph.Nodes)
                    {
                        var mutations = otherNode.Capability.Relations?.Mutations;
                        if (mutations == null)
                            continue;
                        foreach (var mutation in mutations)
                        {
                            if (mutation.AreEqual(requirement))
                            {
                                var otherNodeName = GetNodeName(otherNode, options);
                                edgeList.Add(
                                    (
                                        otherNodeName,
                                        nodeName,
                                        requirement.Description,
                                        requirement.Id
                                    )
                                );
                                // Track edge reference for node/requirement
                                var key = (nodeName, requirement.Id);
                                if (!edgeRefs.TryGetValue(key, out var list))
                                {
                                    list = [];
                                    edgeRefs[key] = list;
                                }
                                list.Add(edgeCounter);
                                edgeCounter++;
                            }
                        }
                    }
                }
            }
        }

        // Render all nodes with requirements in the box, including edge refs only if option enabled
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
            if (options.DisplayNodeRequirements)
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
                            options.DisplayEdgeReference
                            && !isMissing
                            && edgeRefs.TryGetValue(key, out var refList)
                            && refList.Count > 0
                        )
                        {
                            refs = " [" + string.Join(", ", refList) + "]";
                        }
                        lines.Add($"{marker} {req.Description}{refs}");
                    }
                    requirementsText = "<br/>" + string.Join("<br/>", lines);
                }
            }

            sb.AppendLine(nodeName + "[\"" + nodeName + description + requirementsText + "\"]");
        }

        // Render all edges with reference numbers in label only if option enabled
        var edgeNum = 1;
        foreach (var (from, to, label, reqId) in edgeList)
        {
            if (options.DisplayEdgeReference)
            {
                sb.AppendLine(($"{from} -->|\"{label} [{edgeNum}]\"| {to}").ToString());
            }
            else
            {
                sb.AppendLine(($"{from} -->|\"{label}\"| {to}").ToString());
            }
            edgeNum++;
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
}
