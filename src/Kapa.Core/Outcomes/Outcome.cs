using Kapa.Abstractions;
using Kapa.Abstractions.Outcomes;

namespace Kapa.Core.Outcomes;

public record Outcome : IOutcome
{
    public Outcome(string source, OutcomeStatus outcomeStatus)
    {
        Source = source;
        Status = outcomeStatus;
        Kind = Kinds.NoneKind;
        Value = null;
    }

    /// <summary>
    /// Initializes a new instance of <see cref="Outcome"/>
    /// </summary>
    /// <param name="source">The source that has generated the <see cref="Outcome"/>.</param>
    /// <param name="outcomeStatus">The <see cref="OutcomeStatus"/>.</param>
    /// <param name="kind">The <see cref="Kinds"/> of the <see cref="Outcome"/> value.</param>
    /// <param name="value">The <see cref="Outcome"/> value.</param>
    public Outcome(string source, OutcomeStatus outcomeStatus, Kinds kind, object? value)
    {
        Source = source;
        Status = outcomeStatus;
        Kind = kind;
        Value = value;
    }

    public OutcomeStatus Status { get; init; }

    public string? Reason { get; init; }

    public string Source { get; init; }

    public Kinds Kind { get; init; }

    public object? Value { get; init; }
}
