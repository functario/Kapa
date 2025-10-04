namespace Kapa.Fixtures.Capabilities.NoParameters;

[CapabilityType]
public static class StaticCapabilityType
{
    [Capability(nameof(Handle))]
    public static IOutcome Handle()
    {
        return TypedOutcomes.Ok(nameof(StaticCapabilityType));
    }
}
