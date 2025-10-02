using Kapa.Abstractions.Outcomes;
using Kapa.Abstractions.Rules;
using Kapa.Core.Outcomes;

namespace Nono.Capabilities;

[Kapability]
public sealed class KapaB
{
    [KapaStep("description")]
    public IOutcome DoSomething(
        [KapaParam("description str", typeof(StrRule))] string str,
        [KapaParam("description decimalNumber")] decimal decimalNumber,
        [KapaParam("description boolean")] bool boolean,
        [KapaParam("description obj")] object obj,
        [KapaParam("description objNull")] object? objNull,
        [KapaParam("description intCollection")] ICollection<int> intCollection,
        [KapaParam("description boolList")] List<bool> boolList
    )
    {
        return new TypedOutcome<string>(nameof(KapaA), OutcomeStatus.Ok, "ok");
    }
}

public class StrRule : IRule
{
    public string Name => nameof(StrRule);

    public IOutcome Validate()
    {
        if (string.IsNullOrWhiteSpace(Name))
        {
            return new Outcome(Name, OutcomeStatus.Fail);
        }

        return new Outcome(Name, OutcomeStatus.Ok);
    }
}
