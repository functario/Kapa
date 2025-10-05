using Kapa.Abstractions;
using Kapa.Abstractions.Validations;
using Kapa.Core.Extensions;

namespace Kapa.Core.Validations;

public sealed class Fail<TValue> : IOutcome
{
    internal Fail(string source, TValue? value)
        : this(source, value, "") { }

    internal Fail(string source, TValue? value, string reason)
    {
        Source = source;
        Status = OutcomeStatus.Fail;
        ValueInfo = value?.GetType().GetValueInfo();
        Value = value;
        Reason = reason;
    }

    public string Source { get; init; }

    public OutcomeStatus Status { get; init; }

    public string? Reason { get; init; }

    public TValue? Value { get; init; }

    public IValueInfo? ValueInfo { get; init; }
}
