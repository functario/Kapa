using Kapa.Abstractions.Results;
using Kapa.Core.Results;

namespace Kapa.Fixtures.Capabilities.WithTypedOutcomes;

[CapabilityType]
public sealed class OkOrFailOrRulesFailOutcomeCapacity
{
    public OkOrFailOrRulesFailOutcomeCapacity(OutcomeStatus outcomeStatus)
    {
        OutcomeStatus = outcomeStatus;
    }

    public OutcomeStatus OutcomeStatus { get; }

    [Capability(nameof(Handle))]
    public Outcomes<Ok<string>, Fail<string>, RulesFail<string>> Handle()
    {
        return OutcomeStatus switch
        {
            OutcomeStatus.Ok => TypedOutcomes.Ok(nameof(SringOutcomeCapacity), ""),
            OutcomeStatus.Fail => TypedOutcomes.Fail(nameof(SringOutcomeCapacity), ""),
            OutcomeStatus.RulesFail => TypedOutcomes.RulesFail(nameof(SringOutcomeCapacity), ""),
            _ => throw new InvalidOperationException($"Not supporting the case '{OutcomeStatus}'."),
        };
    }
}
