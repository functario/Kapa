using Kapa.Abstractions;
using Kapa.Abstractions.Validations;

namespace Kapa.Core.Validations;

public sealed record Ok : IOutcome
{
    internal Ok(string source)
        : this(source, "") { }

    /// <summary>
    /// Initializes a new instance of <see cref="Ok"/>
    /// </summary>
    /// <param name="source">The source that has generated the <see cref="Ok"/>.</param>
    /// <param name="reason">The reason of the <see cref="OutcomeStatus"/>.</param>
    internal Ok(string source, string reason)
    {
        Source = source;
        Status = OutcomeStatus.Ok;
        Reason = reason;
        ValueInfo = ValueInfos.None();
    }

    public OutcomeStatus Status { get; init; }

    public string? Reason { get; init; }

    public string Source { get; init; }

    public IValueInfo ValueInfo { get; init; }
    public OutcomeTypes OutcomeType => OutcomeTypes.Ok;
}
