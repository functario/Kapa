using Kapa.Abstractions.Actors;
using Kapa.Abstractions.Capabilities;
using Kapa.Abstractions.Graphs;

namespace Kapa.Core.Graphs;

/// <summary>
/// Represents a directed edge from a <see cref="ICapability"/>
/// providing <see cref="IRelations{TGeneratedActor}.Mutations"/> to resolve
/// an another <see cref="ICapability"/>'s <see cref="IRelations{TGeneratedActor}.Requirements"/>.
/// </summary>
/// <param name="FromCapacity">The <see cref="ICapability"/> that provides <see cref="IRelations{TGeneratedActor}.Mutations"/>.</param>
/// <param name="ToCapacity">The <see cref="ICapability"/> that has <see cref="IRelations{TGeneratedActor}.Requirements"/>.</param>
/// <param name="ResolvingMutations">The <see cref="IEffect{TGeneratedActor}"/> from
/// <paramref name="FromCapacity"/> that help satisfy <paramref name="ToCapacity"/>'s
/// <see cref="IRelations{TGeneratedActor}.Requirements"/>.</param>
public sealed record Edge(
    INode FromCapacity,
    INode ToCapacity,
    ICollection<IEffect<IGeneratedActor>> ResolvingMutations
) : IEdge
{ }
