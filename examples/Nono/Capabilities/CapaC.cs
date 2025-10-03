using Kapa.Abstractions.Outcomes;
using Kapa.Core.Outcomes;

namespace Nono.Capabilities;

[CapabilityType]
public sealed class CapaC
{
    [Capability("description")]
    public IOutcome DoSomething()
    {
        return new TypedOutcome<string>(nameof(CapaA), OutcomeStatus.Ok, "value");
    }
}
