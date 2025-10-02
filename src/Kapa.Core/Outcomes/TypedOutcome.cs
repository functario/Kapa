using Kapa.Abstractions.Outcomes;

namespace Kapa.Core.Outcomes;

public record TypedOutcome<T> : Outcome, ITypedOutcome<T>
{
    public TypedOutcome(string kapaStepName, OutcomeStatus status, T? value)
        : base(kapaStepName, status)
    {
        Value = value;
    }

    public T? Value { get; init; }
}
