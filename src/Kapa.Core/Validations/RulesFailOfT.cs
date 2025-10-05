using Kapa.Abstractions;
using Kapa.Abstractions.Validations;
using Kapa.Core.Extensions;

namespace Kapa.Core.Validations;

public sealed class RulesFail<TValue> : IOutcome
{
    internal RulesFail(string source, TValue? value)
        : this(source, value, "") { }

    internal RulesFail(string source, TValue? value, string reason)
    {
        Source = source;
        Status = OutcomeStatus.RulesFail;
        Reason = reason;
        ValueInfo = value?.GetType().GetValueInfo();
        Value = value;
    }

    public string Source { get; init; }

    public OutcomeStatus Status { get; init; }

    public string? Reason { get; init; }

    public TValue? Value { get; init; }
    public IValueInfo? ValueInfo { get; init; }

    public OutcomeTypes OutcomeType => OutcomeTypes.RulesFail | OutcomeTypes.Generic;
}
