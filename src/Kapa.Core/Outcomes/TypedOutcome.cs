using Kapa.Abstractions.Outcomes;

namespace Kapa.Core.Outcomes;

public record TypedOutcome<T> : Outcome, ITypedOutcome<T>
{
    public TypedOutcome(string capabilityName, OutcomeStatus status, T? value)
        : base(capabilityName, status)
    {
        Value = value;
    }

    public T? Value { get; init; }
}
