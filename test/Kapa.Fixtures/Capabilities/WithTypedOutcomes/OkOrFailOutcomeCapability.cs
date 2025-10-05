using System.Reflection;

namespace Kapa.Fixtures.Capabilities.WithTypedOutcomes;

[CapabilityType]
public sealed class OkOrFailOutcomeCapability
{
    public OkOrFailOutcomeCapability(bool isOk = true)
    {
        IsOk = isOk;
    }

    public bool IsOk { get; }

    [Capability(nameof(Handle))]
    public Outcomes<Ok, Fail> Handle()
    {
        if (IsOk)
        {
            return TypedOutcomes.Ok(MethodInfo.GetCurrentMethod(), "reason");
        }

        return TypedOutcomes.Fail(MethodInfo.GetCurrentMethod(), "reason");
    }
}
