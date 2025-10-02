using Kapa.Abstractions.Outcomes;
using Kapa.Core.Outcomes;

namespace Nono.Capabilities;

[Kapability]
public sealed class KapaB
{
    [KapaStep("description")]
    public IOutcome DoSomething()
    {
        return new TypedOutcome<string>(nameof(KapaA), OutcomeStatus.Ok, "ok");
    }
}
