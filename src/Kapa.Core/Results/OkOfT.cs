using Kapa.Abstractions;
using Kapa.Abstractions.Extensions;
using Kapa.Abstractions.Results;

namespace Kapa.Core.Results;

public sealed class Ok<TValue> : IOutcome
{
    internal Ok(string source, TValue? value)
        : this(source, value, "") { }

    internal Ok(string source, TValue? value, string reason)
    {
        Source = source;
        Kind = value?.GetType().InferKind() ?? Kinds.NoneKind;
        Status = OutcomeStatus.Ok;
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
