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
        Value = null;
    }

    /// <summary>
    /// Initializes a new instance of <see cref="Outcome"/>
    /// </summary>
    /// <param name="source">The source that has generated the <see cref="Outcome"/>.</param>
    /// <param name="outcomeStatus">The <see cref="OutcomeStatus"/>.</param>
    /// <param name="value">The <see cref="Outcome"/> value.</param>
    public Outcome(string source, OutcomeStatus outcomeStatus, object? value)
    {
        Source = source;
        Status = outcomeStatus;

        Kind = value?.GetType().InferKind() ?? Kinds.NoneKind;
        if (value is not null && Kind == Kinds.NoneKind)
        {
            throw new InvalidOperationException(
                $"The {nameof(Kinds)} of the value typed '{value.GetType()}' was not infered."
            );
        }

        Value = value;
    }

    public OutcomeStatus Status { get; init; }

    public string? Reason { get; init; }

    public string Source { get; init; }

    public Kinds Kind { get; init; }

    public object? Value { get; init; }
}
