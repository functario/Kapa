using Kapa.Abstractions;
using Kapa.Abstractions.Validations;

namespace Kapa.Core.Validations;

public record RulesFail : IOutcome
{
    internal RulesFail(string source)
        : this(source, "") { }

    /// <summary>
    /// Initializes a new instance of <see cref="RulesFail"/>
    /// </summary>
    /// <param name="source">The source that has generated the <see cref="RulesFail"/>.</param>
    /// <param name="reason">The reason of the <see cref="OutcomeStatus"/>.</param>
    internal RulesFail(string source, string reason)
    {
        Source = source;
        Status = OutcomeStatus.RulesFail;
        Reason = reason;
        ValueInfo = ValueInfos.None();
    }

    public OutcomeStatus Status { get; init; }

    public string? Reason { get; init; }

    public string Source { get; init; }

    public IValueInfo ValueInfo { get; init; }

    public OutcomeTypes OutcomeType => OutcomeTypes.RulesFail;
}
