using Kapa.Core.Validations;

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
    public Outcomes<Ok<string>, Fail<string>> Handle()
    {
        if (IsOk)
        {
            return TypedOutcomes.Ok(nameof(OkOrFailOutcomeCapacity), "value", "reason");
        }

        return TypedOutcomes.Fail(nameof(OkOrFailOutcomeCapacity), "value", "reason");
    }
}
