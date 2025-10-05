using System.Reflection;

namespace Kapa.Fixtures.Capabilities.WithTypedOutcomes;

[CapabilityType]
public sealed class OkOfTCapability
{
    [Capability(nameof(Handle))]
    public Ok<string> Handle()
    {
        return TypedOutcomes.Ok(MethodInfo.GetCurrentMethod(), "value", "reason");
    }
}
