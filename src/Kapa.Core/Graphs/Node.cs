using Kapa.Abstractions.Actors;
using Kapa.Abstractions.Capabilities;
using Kapa.Abstractions.Graphs;

namespace Kapa.Core.Graphs;

/// <summary>
/// A <see cref="Node"/> in the dependency graph
/// with <paramref name="Mutations"/> and <paramref name="Requirements"/>.
/// </summary>
/// <param name="Capability">The <see cref="ICapability"/> associated to this node .</param>
/// <param name="Mutations">The <see cref="IEffect{TGeneratedActor}"/> provided by this <see cref="ICapability"/>.</param>
/// <param name="Requirements">The <see cref="IEffect{TGeneratedActor}"/> that must be satisfied for this <see cref="ICapability"/>.</param>
public sealed record Node(
    ICapability Capability,
    ICollection<IEffect<IGeneratedActor>> Mutations,
    ICollection<IEffect<IGeneratedActor>> Requirements
) : INode
{ }
