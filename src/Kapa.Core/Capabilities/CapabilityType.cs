using Kapa.Abstractions.Capabilities;

namespace Kapa.Core.Capabilities;

/// <inheritdoc/>
public class CapabilityType : ICapabilityType
{
    public CapabilityType(ICollection<ICapability> capabilities)
    {
        Capabilities = [.. capabilities];
    }

    public IReadOnlyCollection<ICapability> Capabilities { get; }
}
