namespace Kapa.Fixtures.Capabilities;

[CapabilityType]
public sealed class CapabilityWithAllParemeterTypes
{
    [Capability(nameof(Handle))]
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "Style",
        "IDE0060:Remove unused parameter",
        Justification = "For testing"
    )]
    public IOutcome Handle(
        [Parameter("description str")] string str,
        [Parameter("description decimalNumber")] decimal decimalNumber,
        [Parameter("description decimalNumber")] int integerNumber,
        [Parameter("description obj")] object obj,
        [Parameter("description intCollection")] ICollection<int> intCollection,
        [Parameter("description boolean")] bool boolean
    )
    {
        return new Outcome(nameof(CapabilityWithAllParemeterTypes), OutcomeStatus.Ok);
    }
}
