using Kapa.Abstractions.Results;
using Kapa.Fixtures.Rules;

namespace Kapa.Fixtures.Capabilities.WithRules;

[CapabilityType]
public sealed class OneRuleOnAllParameterTypes
{
    [Capability(nameof(Handle))]
    public IOutcome Handle(
        [Parameter("description str", typeof(GenericRule))] string str,
        [Parameter("description decimalNumber", typeof(GenericRule))] decimal decimalNumber,
        [Parameter("description decimalNumber", typeof(GenericRule))] int integerNumber,
        [Parameter("description obj", typeof(GenericRule))] object obj,
        [Parameter("description intCollection", typeof(GenericRule))]
            ICollection<int> intCollection,
        [Parameter("description boolean", typeof(GenericRule))] bool boolean
    )
    {
        return new Outcome(nameof(OneRuleOnAllParameterTypes), OutcomeStatus.Ok);
    }
}
