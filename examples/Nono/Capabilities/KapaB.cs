using Kapa.Abstractions.Outcomes;
using Kapa.Core.Outcomes;

namespace Nono.Capabilities;

[Kapability]
public sealed class KapaB
{
    // Recuperer le type de Outcome
    // nom, type, description des params, rule
    [KapaStep("description")]
    public IOutcome DoSomething(
        [KapaParam("description str")] string str,
        long longNumber,
        int intNumber,
        double doubleNumber,
        decimal decimalNumber,
        float floatNumber,
        bool boolean,
        object obj,
        object? objNull,
        ICollection<int> intCollection,
        List<bool> boolList
    )
    {
        return new TypedOutcome<string>(nameof(KapaA), OutcomeStatus.Ok, "ok");
    }
}
