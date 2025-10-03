using Kapa.Abstractions.Outcomes;

namespace Kapa.Core.Outcomes;

public record TypedOutcome<T> : Outcome, ITypedOutcome<T>
{
    public TypedOutcome(string source, OutcomeStatus status, T? value)
        : base(source, status)
    {
        Value = value;
    }

    public T? Value { get; init; }
}
