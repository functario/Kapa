using Kapa.Fixtures.Rules;

namespace Kapa.Fixtures.Capabilities.WithRules;

[CapabilityType]
public sealed class ManyRulesOnAllParameterTypes
{
    [Capability(nameof(Handle))]
    public IOutcome Handle(
        [Parameter(
            "description str",
            typeof(GenericRule),
            typeof(GenericRule),
            typeof(GenericRule)
        )]
            string str,
        [Parameter(
            "description decimalNumber",
            typeof(GenericRule),
            typeof(GenericRule),
            typeof(GenericRule)
        )]
            decimal decimalNumber,
        [Parameter(
            "description decimalNumber",
            typeof(GenericRule),
            typeof(GenericRule),
            typeof(GenericRule)
        )]
            int integerNumber,
        [Parameter(
            "description obj",
            typeof(GenericRule),
            typeof(GenericRule),
            typeof(GenericRule)
        )]
            object obj,
        [Parameter(
            "description intCollection",
            typeof(GenericRule),
            typeof(GenericRule),
            typeof(GenericRule)
        )]
            ICollection<int> intCollection,
        [Parameter(
            "description boolean",
            typeof(GenericRule),
            typeof(GenericRule),
            typeof(GenericRule)
        )]
            bool boolean
    )
    {
        return new Outcome(nameof(ManyRulesOnAllParameterTypes), OutcomeStatus.Ok);
    }
}
