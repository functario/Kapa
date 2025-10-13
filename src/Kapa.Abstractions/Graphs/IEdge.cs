using Kapa.Abstractions.Actors;

namespace Kapa.Abstractions.Graphs;

public interface IEdge
{
    INode FromCapacity { get; }
    ICollection<IEffect<IGeneratedActor>> ResolvingMutations { get; }
    INode ToCapacity { get; }
    int Index { get; }
}
