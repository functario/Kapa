using Kapa.Abstractions.Actors;
using Kapa.Abstractions.Capabilities;
using Kapa.Abstractions.Graphs;

namespace Kapa.Core.Graphs;

/// <summary>
/// A <see cref="Node"/> in the dependency graph
/// with <see cref="IMutation{TGeneratedActor}"/> and <see cref="IRequirement{TGeneratedActor}"/>.
/// </summary>
/// <param name="Capability">The <see cref="ICapability"/> associated to this node .</param>
/// <param name="Mutations">The <see cref="IMutation{TGeneratedActor}"/> provided by this <see cref="ICapability"/>.</param>
/// <param name="Requirements">The <see cref="IRequirement{TGeneratedActor}"/> that must be satisfied for this <see cref="ICapability"/>.</param>
public sealed record Node(
    ICapability Capability,
    ICollection<IMutation<IGeneratedActor>> Mutations,
    ICollection<IRequirement<IGeneratedActor>> Requirements
) : INode { }
