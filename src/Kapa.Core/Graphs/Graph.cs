using Kapa.Abstractions.Graphs;

namespace Kapa.Core.Graphs;

public sealed class Graph : IGraph
{
    public Graph(IReadOnlyCollection<INode> nodes)
    {
        Nodes = nodes;
    }

    public IReadOnlyCollection<INode> Nodes { get; }

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
