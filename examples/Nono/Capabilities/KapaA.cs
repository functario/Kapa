using Kapa.Abstractions.Outcomes;
using Kapa.Core.Outcomes;

namespace Nono.Capabilities;

[Kapability]
public sealed class KapaA
{
    [KapaStep("description", "name")]
    public IOutcome DoSomething()
    {
        return new Outcome(nameof(KapaA), OutcomeStatus.Ok);
    }
}
