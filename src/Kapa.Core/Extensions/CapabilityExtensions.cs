using Kapa.Abstractions.Capabilities;
using Kapa.Abstractions.Graphs;
using Kapa.Core.Graphs;

namespace Kapa.Core.Extensions;

public static class CapabilityExtensions
{
    public static INode ToNode(this ICapability capability)
    {
        ArgumentNullException.ThrowIfNull(capability);
        var mutations = capability.Relations?.Mutations ?? [];
        var requirements = capability.Relations?.Requirements ?? [];
        return new Node(capability.GetType(), mutations, requirements);
    }
}
