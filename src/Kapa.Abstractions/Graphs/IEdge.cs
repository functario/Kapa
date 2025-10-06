using Kapa.Abstractions.Prototypes;

namespace Kapa.Abstractions.Graphs;

public interface IEdge
{
    INode FromCapacity { get; init; }
    ICollection<IMutation<IGeneratedPrototype>> ResolvingMutations { get; init; }
    INode ToCapacity { get; init; }
}
