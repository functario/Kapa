using Kapa.Abstractions.Prototypes;

namespace Kapa.Abstractions.Graphs;

public interface INode
{
    ICollection<IMutation<IGeneratedPrototype>> Mutations { get; init; }
    ICollection<IRequirement<IGeneratedPrototype>> Requirements { get; init; }
    Type Type { get; init; }
}
