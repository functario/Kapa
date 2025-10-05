using Kapa.Abstractions.Validations;
using Kapa.Core.Validations;

namespace Kapa.Fixtures.Capabilities.NoParameters;

// Intentionally missing [CapabilityType] attribute for testing
public sealed class NotCapabilityType
{
    public IOutcome Handle()
    {
        return TypedOutcomes.Ok(nameof(NotCapabilityType));
    }
}
