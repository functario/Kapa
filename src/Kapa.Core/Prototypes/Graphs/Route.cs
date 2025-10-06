using Kapa.Abstractions.Prototypes;

namespace Kapa.Core.Prototypes.Graphs;

/// <summary>
/// Represents a valid path through the dependency graph.
/// </summary>
/// <param name="Edges">The ordered sequence of edges composing this route.</param>
public sealed record Route<TPrototype>(IReadOnlyList<Edge<TPrototype>> Edges)
    where TPrototype : IPrototype
{ }
