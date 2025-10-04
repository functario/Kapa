using Kapa.Abstractions;
using Kapa.Abstractions.Results;

namespace Kapa.Core.Results;

public sealed class Fail<TValue> : IOutcome
{
    internal Fail(string source, TValue? value)
        : this(source, value, "") { }

    internal Fail(string source, TValue? value, string reason)
    {
        Source = source;
        Status = OutcomeStatus.Fail;
        Kind = value?.GetType().InferKind() ?? Kinds.NoneKind;
        if (value is not null && Kind == Kinds.NoneKind)
        {
            throw new InvalidOperationException(
                $"The {nameof(Kinds)} of the value typed '{value.GetType()}' was not infered."
            );
        }

        Value = value;
        Reason = reason;
    }

    public string Source { get; init; }

    public OutcomeStatus Status { get; init; }

    public string? Reason { get; init; }

    public Kinds Kind { get; init; }

    public TValue? Value { get; init; }
}
