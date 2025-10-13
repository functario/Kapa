using Kapa.Abstractions.Capabilities;

namespace Kapa.Abstractions.Graphs;

public interface INode : IEquatable<INode>
{
    ICapability Capability { get; }
}
