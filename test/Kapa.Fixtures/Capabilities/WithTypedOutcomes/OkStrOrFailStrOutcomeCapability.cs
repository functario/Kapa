namespace Kapa.Fixtures.Capabilities.WithTypedOutcomes;

[CapabilityType]
public sealed class OkStrOrFailStrOutcomeCapability
{
    public OkStrOrFailStrOutcomeCapability(bool isOk = true)
    {
        IsOk = isOk;
    }

    public bool IsOk { get; }

    [Capability(nameof(Handle))]
    public Outcomes<Ok<string>, Fail<string>> Handle()
    {
        if (IsOk)
        {
            return TypedOutcomes.Ok(nameof(OkStrOrFailStrOutcomeCapability), "value", "reason");
        }

        return TypedOutcomes.Fail(nameof(OkStrOrFailStrOutcomeCapability), "value", "reason");
    }
}
