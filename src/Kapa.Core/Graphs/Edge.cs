using Kapa.Abstractions.Capabilities;
using Kapa.Abstractions.Graphs;
using Kapa.Abstractions.Prototypes;

namespace Kapa.Core.Graphs;

/// <summary>
/// Represents a directed edge from a <see cref="ICapability"/>
/// providing <see cref="IMutation{TGeneratedPrototype}"/> to resolve
/// an another <see cref="ICapability"/>'s <see cref="IRequirement{TGeneratedPrototype}"/>.
/// </summary>
/// <param name="FromCapacity">The <see cref="ICapability"/> that provides <see cref="IMutation{TGeneratedPrototype}"/>.</param>
/// <param name="ToCapacity">The <see cref="ICapability"/> that has <see cref="IRequirement{TGeneratedPrototype}"/>.</param>
/// <param name="ResolvingMutations">The <see cref="IMutation{TGeneratedPrototype}"/> from
/// <paramref name="FromCapacity"/> that help satisfy <paramref name="ToCapacity"/>'s
/// <see cref="IRequirement{TGeneratedPrototype}"/>.</param>
public sealed record Edge(
    INode FromCapacity,
    INode ToCapacity,
    ICollection<IMutation<IGeneratedPrototype>> ResolvingMutations
) : IEdge
{ }
