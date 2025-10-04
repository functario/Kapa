using Kapa.Abstractions;
using Kapa.Abstractions.Outcomes;

namespace Kapa.Core.Outcomes;

public sealed class Ok<TValue> : IOutcome
{
    internal Ok(string source, OutcomeStatus outcomeStatus, TValue? value)
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

        MyValue = value;
    }

    public string Source { get; init; }

    public OutcomeStatus Status { get; init; }

    public string? Reason { get; init; }

    public Kinds Kind { get; init; }

    public TValue? MyValue { get; init; }
    public object? Value => MyValue;
}
