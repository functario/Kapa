using Kapa.Abstractions.Capabilities;
using Kapa.Abstractions.Prototypes;

namespace Kapa.Abstractions.Graphs;

public interface INode
{
    ICollection<IMutation<IGeneratedPrototype>> Mutations { get; init; }
    ICollection<IRequirement<IGeneratedPrototype>> Requirements { get; init; }
    ICapability Capability { get; init; }
}
