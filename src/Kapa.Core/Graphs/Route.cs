namespace Kapa.Core.Graphs;

/// <summary>
/// Represents a valid path through the dependency <see cref="Graph"/>.
/// </summary>
/// <param name="Edges">The ordered sequence of <see cref="Edge"/> composing this <see cref="Route"/>.</param>
public sealed record Route(IReadOnlyList<Edge> Edges) { }
