namespace Kapa.Fixtures.Capabilities.WithTypedOutcomes;

[CapabilityType]
public sealed class OkOrFailOrRulesFailOutcomeCapability
{
    public OkOrFailOrRulesFailOutcomeCapability(OutcomeStatus outcomeStatus)
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
                nameof(OkOrFailOrRulesFailOutcomeCapability),
                "value",
                "reason"
            ),
            OutcomeStatus.Fail => TypedOutcomes.Fail(
                nameof(OkOrFailOrRulesFailOutcomeCapability),
                "value",
                "reason"
            ),
            OutcomeStatus.RulesFail => TypedOutcomes.RulesFail(
                nameof(OkOrFailOrRulesFailOutcomeCapability),
                "value",
                "reason"
            ),
            _ => throw new InvalidOperationException($"Not supporting the case '{OutcomeStatus}'."),
        };
    }
}
