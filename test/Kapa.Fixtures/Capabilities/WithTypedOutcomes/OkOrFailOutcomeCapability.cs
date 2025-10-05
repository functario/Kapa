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
    public Outcomes<Ok<string>, Fail<string>> Handle()
    {
        if (IsOk)
        {
            return TypedOutcomes.Ok(nameof(OkOrFailOutcomeCapability), "value", "reason");
        }

        return TypedOutcomes.Fail(nameof(OkOrFailOutcomeCapability), "value", "reason");
    }
}
