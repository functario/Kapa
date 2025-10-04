using Kapa.Abstractions;
using Kapa.Abstractions.Results;

namespace Kapa.Core.Results;

public record Outcome : IOutcome
{
    public Outcome(string source, OutcomeStatus outcomeStatus)
    {
        Source = source;
        Status = outcomeStatus;
        Kind = Kinds.NoneKind;
    }

    /// <summary>
    /// Initializes a new instance of <see cref="Outcome"/>
    /// </summary>
    /// <param name="source">The source that has generated the <see cref="Outcome"/>.</param>
    /// <param name="outcomeStatus">The <see cref="OutcomeStatus"/>.</param>
    /// <param name="reason">The reason of the <see cref="OutcomeStatus"/>.</param>
    public Outcome(string source, OutcomeStatus outcomeStatus, string reason)
    {
        Source = source;
        Status = outcomeStatus;
        Reason = reason;
        Kind = Kinds.NoneKind;
    }

    public OutcomeStatus Status { get; init; }

    public string? Reason { get; init; }

    public string Source { get; init; }

    public Kinds Kind { get; init; }
}
