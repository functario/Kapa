namespace Kapa.Fixtures.Capabilities.WithTypedOutcomes;

[CapabilityType]
public sealed class OkOfTCapacity
{
    [Capability(nameof(Handle))]
    public Ok<string> Handle()
    {
        return TypedOutcomes.Ok(nameof(OkOfTCapacity), "ok");
    }
}
