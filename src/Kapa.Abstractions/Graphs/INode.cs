using Kapa.Abstractions.Capabilities;

namespace Kapa.Abstractions.Graphs;

public interface INode
{
    ICapability Capability { get; init; }
}
