namespace Kapa.Fixtures.Capabilities.NoParameters;

[CapabilityType]
public sealed class ManyCapabilities
{
    [Capability(nameof(Handle1))]
    public IOutcome Handle1()
    {
        return TypedOutcomes.Ok(nameof(ManyCapabilities));
    }

    [Capability(nameof(Handle2))]
    public IOutcome Handle2()
    {
        return TypedOutcomes.Ok(nameof(ManyCapabilities));
    }

    [Capability(nameof(Handle3))]
    public IOutcome Handle3()
    {
        return TypedOutcomes.Ok(nameof(ManyCapabilities));
    }
}
