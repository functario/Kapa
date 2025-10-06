using Kapa.Abstractions.Capabilities;

namespace Kapa.Abstractions.Graphs;

public interface IGraph
{
    public IReadOnlyCollection<INode> Nodes { get; }

    public ICollection<IRoute> Resolve(ICollection<INode> includedNodes, int maxRoutes = 50);

    public ICollection<IRoute> Resolve(
        ICollection<INode> includedNodes,
        ICollection<INode> excludedNodes,
        int maxRoutes = 50
    );

    /// <summary>
    /// Reduce the <see cref="IGraph"/> to <see cref="ICapability"/> resolving the dependencies of <paramref name="includedNodes"/>
    /// </summary>
    /// <param name="includedNodes"></param>
    /// <param name="excludedNodes"></param>
    /// <returns></returns>
    public IGraph Reduce(ICollection<INode> includedNodes, ICollection<INode> excludedNodes);
}
