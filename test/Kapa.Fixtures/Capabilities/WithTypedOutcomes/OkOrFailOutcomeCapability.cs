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
            return TypedOutcomes.Ok(nameof(OkOrFailOutcomeCapability), "reason");
        }

        return TypedOutcomes.Fail(nameof(OkOrFailOutcomeCapability), "reason");
    }
}
