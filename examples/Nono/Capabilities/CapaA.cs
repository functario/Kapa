using Kapa.Abstractions.Outcomes;
using Kapa.Core.Outcomes;

namespace Nono.Capabilities;

[CapabilityType]
public sealed class CapaA
{
    [Capability("description")]
    public IOutcome DoSomething()
    {
        return new Outcome(nameof(CapaA), OutcomeStatus.Ok);
    }
}
