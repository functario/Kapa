using Kapa.Abstractions.Outcomes;
using Kapa.Core.Outcomes;

namespace Nono.Capabilities;

[Capability]
public sealed class KapaB
{
    [CapabilityFunc("description")]
    public IOutcome DoSomething()
    {
        return new TypedOutcome<string>(nameof(KapaA), OutcomeStatus.Ok, "ok");
    }
}
