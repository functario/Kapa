using Kapa.Abstractions.Outcomes;

namespace Kapa.Core.Outcomes;

public record Outcome : IOutcome
{
    /// <summary>
    /// Initializes a new instance of <see cref="Outcome"/>.
    /// </summary>
    /// <param name="source">The source that has generated the <see cref="IOutcome"/>.</param>
    /// <param name="outcomeStatus">The <see cref="OutcomeStatus"/>.</param>
    public Outcome(string source, OutcomeStatus outcomeStatus)
    {
        Source = source;
        Status = outcomeStatus;
    }

    public OutcomeStatus Status { get; init; }

    public string? Reason { get; init; }

    public string Source { get; init; }
}
