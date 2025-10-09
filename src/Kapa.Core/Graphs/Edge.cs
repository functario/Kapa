using Kapa.Abstractions.Actors;
using Kapa.Abstractions.Capabilities;
using Kapa.Abstractions.Graphs;

namespace Kapa.Core.Graphs;

/// <summary>
/// Represents a directed edge from a <see cref="ICapability"/>
/// providing <see cref="IMutation{TGeneratedActor}"/> to resolve
/// an another <see cref="ICapability"/>'s <see cref="IRequirement{TGeneratedActor}"/>.
/// </summary>
/// <param name="FromCapacity">The <see cref="ICapability"/> that provides <see cref="IMutation{TGeneratedActor}"/>.</param>
/// <param name="ToCapacity">The <see cref="ICapability"/> that has <see cref="IRequirement{TGeneratedActor}"/>.</param>
/// <param name="ResolvingMutations">The <see cref="IMutation{TGeneratedActor}"/> from
/// <paramref name="FromCapacity"/> that help satisfy <paramref name="ToCapacity"/>'s
/// <see cref="IRequirement{TGeneratedActor}"/>.</param>
public sealed record Edge(
    INode FromCapacity,
    INode ToCapacity,
    ICollection<IMutation<IGeneratedActor>> ResolvingMutations
) : IEdge { }
