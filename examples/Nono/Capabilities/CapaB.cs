using Kapa.Abstractions.Outcomes;
using Kapa.Abstractions.Rules;
using Kapa.Core.Outcomes;

namespace Nono.Capabilities;

[CapabilityType]
public sealed class CapaB
{
    [Capability("description")]
    public IOutcome DoSomething(
        [Parameter("description str", typeof(StrRule))] string str,
        [Parameter("description decimalNumber")] decimal decimalNumber,
        [Parameter("description boolean")] bool boolean,
        [Parameter("description obj")] object obj,
        [Parameter("description objNull")] object? objNull,
        [Parameter("description intCollection")] ICollection<int> intCollection,
        [Parameter("description boolList")] List<bool> boolList
    )
    {
        return new TypedOutcome<string>(nameof(CapaA), OutcomeStatus.Ok, "ok");
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
