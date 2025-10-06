namespace Kapa.Abstractions.Graphs;

public interface IGraph
{
    IReadOnlyCollection<INode> Nodes { get; }

    ICollection<IRoute> Resolve(ICollection<INode> orderedWaypoints, int maxRoutes = 50);
    ICollection<IRoute> Resolve(
        ICollection<INode> orderedWaypoints,
        ICollection<INode> excludedNodes,
        int maxRoutes = 50
    );

    IGraph FocusGraph(ICollection<INode> orderedWaypoints, ICollection<INode> excludedNodes);
}
