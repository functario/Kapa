using Kapa.Abstractions;
using Kapa.Abstractions.Validations;

namespace Kapa.Core.Validations;

public record Fail : IFail
{
    internal Fail(string source)
        : this(source, "") { }

    /// <summary>
    /// Initializes a new instance of <see cref="Fail"/>
    /// </summary>
    /// <param name="source">The source that has generated the <see cref="Fail"/>.</param>
    /// <param name="reason">The reason of the <see cref="OutcomeStatus"/>.</param>
    internal Fail(string source, string reason)
    {
        Source = source;
        Status = OutcomeStatus.Fail;
        Reason = reason;
        ValueInfo = ValueInfos.None();
    }

    public OutcomeStatus Status { get; init; }

    public string? Reason { get; init; }

    public string Source { get; init; }

    public IValueInfo ValueInfo { get; init; }

    public OutcomeTypes OutcomeType => OutcomeTypes.Fail;
}
