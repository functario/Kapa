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
            OutcomeStatus.Ok => TypedOutcomes.Ok(nameof(OkOrFailOrRulesFailOutcomeCapacity), ""),
            OutcomeStatus.Fail => TypedOutcomes.Fail(
                nameof(OkOrFailOrRulesFailOutcomeCapacity),
                ""
            ),
            OutcomeStatus.RulesFail => TypedOutcomes.RulesFail(
                nameof(OkOrFailOrRulesFailOutcomeCapacity),
                ""
            ),
            _ => throw new InvalidOperationException($"Not supporting the case '{OutcomeStatus}'."),
        };
    }
}
