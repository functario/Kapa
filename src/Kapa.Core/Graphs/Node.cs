using Kapa.Abstractions.Capabilities;
using Kapa.Abstractions.Graphs;
using Kapa.Abstractions.Prototypes;

namespace Kapa.Core.Graphs;

/// <summary>
/// Represents a <see cref="ICapability"/> as a node in the dependency graph
/// with <see cref="IMutation{THasTrait}"/> and <see cref="IRequirement{THasTrait}"/>.
/// </summary>
/// <param name="Type">The <see cref="ICapability"/> class type this node represents.</param>
/// <param name="Mutations">The <see cref="IMutation{THasTrait}"/> provided by this <see cref="ICapability"/>.</param>
/// <param name="Requirements">The <see cref="IRequirement{THasTrait}"/> that must be satisfied for this <see cref="ICapability"/>.</param>
public sealed record Node(
    Type Type,
    ICollection<IMutation<IGeneratedPrototype>> Mutations,
    ICollection<IRequirement<IGeneratedPrototype>> Requirements
) : INode
{ }
