using Kapa.Abstractions.Capabilities;
using Kapa.Abstractions.Graphs;
using Kapa.Abstractions.Prototypes;

namespace Kapa.Core.Graphs;

/// <summary>
/// Represents a directed edge from a <see cref="ICapability"/>
/// providing <see cref="IMutation{THasTrait}"/> to resolve
/// an another <see cref="ICapability"/>'s <see cref="IRequirement{THasTrait}"/>.
/// </summary>
/// <param name="FromCapacity">The <see cref="ICapability"/> that provides <see cref="IMutation{THasTrait}"/>.</param>
/// <param name="ToCapacity">The <see cref="ICapability"/> that has <see cref="IRequirement{THasTrait}"/>.</param>
/// <param name="ResolvingMutations">The <see cref="IMutation{THasTrait}"/> from
/// <paramref name="FromCapacity"/> that help satisfy <paramref name="ToCapacity"/>'s
/// <see cref="IRequirement{THasTrait}"/>.</param>
public sealed record Edge(
    INode FromCapacity,
    INode ToCapacity,
    ICollection<IMutation<IHasTrait>> ResolvingMutations
) : IEdge
{ }
