using Kapa.Abstractions.Prototypes;

namespace Kapa.Abstractions.Graphs;

public interface INode
{
    ICollection<IMutation<IHasTrait>> Mutations { get; init; }
    ICollection<IRequirement<IHasTrait>> Requirements { get; init; }
    Type Type { get; init; }
}
