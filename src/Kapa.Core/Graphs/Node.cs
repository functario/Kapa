using Kapa.Abstractions.Capabilities;
using Kapa.Abstractions.Graphs;
using Kapa.Abstractions.Prototypes;

namespace Kapa.Core.Graphs;

/// <summary>
/// A <see cref="Node"/> in the dependency graph
/// with <see cref="IMutation{TGeneratedPrototype}"/> and <see cref="IRequirement{TGeneratedPrototype}"/>.
/// </summary>
/// <param name="Capability">The <see cref="ICapability"/> associated to this node .</param>
/// <param name="Mutations">The <see cref="IMutation{TGeneratedPrototype}"/> provided by this <see cref="ICapability"/>.</param>
/// <param name="Requirements">The <see cref="IRequirement{TGeneratedPrototype}"/> that must be satisfied for this <see cref="ICapability"/>.</param>
public sealed record Node(
    ICapability Capability,
    ICollection<IMutation<IGeneratedPrototype>> Mutations,
    ICollection<IRequirement<IGeneratedPrototype>> Requirements
) : INode
{ }
