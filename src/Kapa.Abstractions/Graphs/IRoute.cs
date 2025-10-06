namespace Kapa.Abstractions.Graphs;

public interface IRoute
{
    IReadOnlyList<IEdge> Edges { get; init; }
}
