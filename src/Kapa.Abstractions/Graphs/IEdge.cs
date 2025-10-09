using Kapa.Abstractions.Actors;

namespace Kapa.Abstractions.Graphs;

public interface IEdge
{
    INode FromCapacity { get; init; }
    ICollection<IMutation<IGeneratedActor>> ResolvingMutations { get; init; }
    INode ToCapacity { get; init; }
}
