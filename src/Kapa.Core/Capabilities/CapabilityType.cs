using Kapa.Abstractions.Capabilities;

namespace Kapa.Core.Capabilities;

public class CapabilityType : ICapabilityType
{
    public CapabilityType(ICollection<ICapability> capabilities)
    {
        Capabilities = [.. capabilities];
    }

    public IReadOnlyCollection<ICapability> Capabilities { get; }
}
