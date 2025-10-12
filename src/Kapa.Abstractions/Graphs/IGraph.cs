using Kapa.Abstractions.Capabilities;

namespace Kapa.Abstractions.Graphs;

public interface IGraph
{
    public IReadOnlyCollection<INode> Nodes { get; }

    /// <summary>
    /// Reduce the <see cref="IGraph"/> to <see cref="ICapability"/> resolving the dependencies of <paramref name="includedNodes"/>.
    /// </summary>
    /// <param name="includedNodes">The <see cref="INode"/> to included in order.</param>
    /// <param name="excludedNodes">The <see cref="INode"/> to excluded.</param>
    /// <returns>A reduced <see cref="IGraph"/>.</returns>
    public IGraph Reduce(IReadOnlyList<INode> includedNodes, IReadOnlyList<INode> excludedNodes);
}
