using Kapa.Abstractions.Graphs;

namespace Kapa.Core.Graphs;

/// <summary>
/// Represents a valid path through the dependency <see cref="Graph"/>.
/// </summary>
/// <param name="Index">An arbritary index to reference the <see cref="IRoute"/>
/// solenely relevant in the current <see cref="IGraph"/> instance context.</param>
/// <param name="Edges">The ordered sequence of <see cref="Edge"/> composing this <see cref="Route"/>.</param>
public sealed record Route(int Index, IReadOnlyList<IEdge> Edges) : IRoute { }
