namespace Kapa.Fixtures.Capabilities.WithTypedOutcomes;

[CapabilityType]
public sealed class OkStrOrFailStrOrRulesFailStrOutcomeCapability
{
    public OkStrOrFailStrOrRulesFailStrOutcomeCapability(OutcomeStatus outcomeStatus)
    {
        OutcomeStatus = outcomeStatus;
    }

    public OutcomeStatus OutcomeStatus { get; }

    [Capability(nameof(Handle))]
    public Outcomes<Ok<string>, Fail<string>, RulesFail<string>> Handle()
    {
        return OutcomeStatus switch
        {
            OutcomeStatus.Ok => TypedOutcomes.Ok(
                nameof(OkStrOrFailStrOrRulesFailStrOutcomeCapability),
                "value",
                "reason"
            ),
            OutcomeStatus.Fail => TypedOutcomes.Fail(
                nameof(OkStrOrFailStrOrRulesFailStrOutcomeCapability),
                "value",
                "reason"
            ),
            OutcomeStatus.RulesFail => TypedOutcomes.RulesFail(
                nameof(OkStrOrFailStrOrRulesFailStrOutcomeCapability),
                "value",
                "reason"
            ),
            _ => throw new InvalidOperationException($"Not supporting the case '{OutcomeStatus}'."),
        };
    }
}
