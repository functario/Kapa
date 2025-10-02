using Kapa.Abstractions.Outcomes;
using Kapa.Core.Outcomes;

namespace Nono.Capabilities;

[Capability]
public sealed class KapaA
{
    [CapabilityFunc("description", "name")]
    public IOutcome DoSomething()
    {
        return new Outcome(nameof(KapaA), OutcomeStatus.Ok);
    }
}
