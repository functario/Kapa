namespace Kapa.Fixtures.Capabilities.WithTypedOutcomes;

[CapabilityType]
public sealed class OkOrFailOutcomeCapacity
{
    public OkOrFailOutcomeCapacity(bool isOk = true)
    {
        IsOk = isOk;
    }

    public bool IsOk { get; }

    [Capability(nameof(Handle))]
    public Results<Ok<string>, Fail<string>> Handle()
    {
        if (IsOk)
        {
            return TypedOutcomes.Ok(nameof(SringOutcomeCapacity), "");
        }

        return TypedOutcomes.Fail(nameof(SringOutcomeCapacity), "");
    }
}
