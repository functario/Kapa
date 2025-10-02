using Kapa.Abstractions.Outcomes;
using Kapa.Core.Outcomes;

namespace Nono.Capabilities;

[Kapability]
public sealed class KapaA
{
    [KapaStep("description", "title with spaces")]
    public IOutcome DoSomething()
    {
        return new Outcome(nameof(KapaA), OutcomeStatus.Ok);
    }
}
