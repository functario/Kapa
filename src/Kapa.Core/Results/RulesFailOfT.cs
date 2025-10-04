using Kapa.Abstractions;
using Kapa.Abstractions.Extensions;
using Kapa.Abstractions.Results;

namespace Kapa.Core.Results;

public sealed class RulesFail<TValue> : IOutcome
{
    internal RulesFail(string source, TValue? value)
        : this(source, value, "") { }

    internal RulesFail(string source, TValue? value, string reason)
    {
        Source = source;
        Status = OutcomeStatus.RulesFail;
        Kind = value?.GetType().InferKind() ?? Kinds.NoneKind;
        Reason = reason;
        if (value is not null && Kind == Kinds.NoneKind)
        {
            throw new InvalidOperationException(
                $"The {nameof(Kinds)} of the value typed '{value.GetType()}' was not infered."
            );
        }

        Value = value;
    }

    public string Source { get; init; }

    public OutcomeStatus Status { get; init; }

    public string? Reason { get; init; }

    public Kinds Kind { get; init; }

    public TValue? Value { get; init; }
}
