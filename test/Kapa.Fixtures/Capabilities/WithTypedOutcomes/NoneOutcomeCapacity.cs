using Kapa.Core.Extensions;

namespace Kapa.Fixtures.Capabilities.WithTypedOutcomes;

[CapabilityType]
public sealed class NoneOutcomeCapacity
{
    [Capability(nameof(Handle))]
    public IOutcome Handle()
    {
        return nameof(SringOutcomeCapacity).Ok();
    }
}
