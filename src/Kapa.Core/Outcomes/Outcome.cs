using Kapa.Abstractions.Outcomes;

namespace Kapa.Core.Outcomes;

public record Outcome : IOutcome
{
    public Outcome(string capabilityName, OutcomeStatus outcomeStatus)
    {
        CapabilityName = capabilityName;
        Status = outcomeStatus;
    }

    public OutcomeStatus Status { get; init; }

    public string? Reason { get; init; }

    public string CapabilityName { get; init; }
}
