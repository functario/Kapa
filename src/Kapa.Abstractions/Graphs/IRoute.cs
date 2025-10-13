namespace Kapa.Abstractions.Graphs;

public interface IRoute : IEquatable<IRoute>
{
    /// <summary>
    /// An arbritary index to reference the <see cref="IRoute"/> solenely
    /// relevant in the current <see cref="IGraph"/> instance context.
    /// </summary>
    int Index { get; }
    IReadOnlyList<IEdge> Edges { get; }
}
