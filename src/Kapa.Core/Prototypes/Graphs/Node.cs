using Kapa.Abstractions.Prototypes;

namespace Kapa.Core.Prototypes.Graphs;

/// <summary>
/// Represents a node in the dependency graph with mutations and requirements.
/// </summary>
/// <param name="Type">The class type this node represents.</param>
/// <param name="Mutations">The mutations provided by this node.</param>
/// <param name="Requirements">The requirements that must be satisfied for this node.</param>
public sealed record Node<TPrototype>(
    Type Type,
    ICollection<IMutation<TPrototype>> Mutations,
    ICollection<IRequirement<TPrototype>> Requirements
)
    where TPrototype : IPrototype
{ }
