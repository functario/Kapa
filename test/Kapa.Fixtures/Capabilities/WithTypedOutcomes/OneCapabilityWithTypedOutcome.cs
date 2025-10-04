using Kapa.Core.Extensions;

namespace Kapa.Fixtures.Capabilities.WithTypedOutcomes;

[CapabilityType]
public sealed class OneCapabilityWithTypedOutcome
{
    [Capability(nameof(Handle))]
    public IOutcome Handle()
    {
        return nameof(OneCapabilityWithTypedOutcome)
            .ToOkTypedOutcome(nameof(OneCapabilityWithTypedOutcome));
    }
}
