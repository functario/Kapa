using Kapa.Abstractions.Actors;

namespace Kapa.Abstractions.Graphs;

public interface IEdge
{
    INode FromCapacity { get; init; }
    ICollection<IEffect<IGeneratedActor>> ResolvingMutations { get; init; }
    INode ToCapacity { get; init; }
}
