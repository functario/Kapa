using Kapa.Abstractions.Outcomes;

namespace Kapa.Core.Outcomes;

public record Outcome : IOutcome
{
    public Outcome(string kapaStepName, OutcomeStatus outcomeStatus)
    {
        KapaStepName = kapaStepName;
        Status = outcomeStatus;
    }

    public OutcomeStatus Status { get; init; }

    public string? Reason { get; init; }

    public string KapaStepName { get; init; }
}
