using Kapa.Abstractions.Prototypes;

namespace Kapa.Core.Prototypes.Graphs;

/// <summary>
/// Represents a directed edge from a mutation provider to a requirement consumer.
/// </summary>
/// <param name="FromNode">The node that provides mutations.</param>
/// <param name="ToNode">The node that has requirements.</param>
/// <param name="ResolvingMutations">The mutations from FromNode that help satisfy ToNode's requirements.</param>
public sealed record Edge<TPrototype>(
    Node<TPrototype> FromNode,
    Node<TPrototype> ToNode,
    ICollection<IMutation<TPrototype>> ResolvingMutations
)
    where TPrototype : IPrototype
{ }
