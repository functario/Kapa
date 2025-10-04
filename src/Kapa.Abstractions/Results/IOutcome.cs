namespace Kapa.Abstractions.Results;

/// <summary>
/// The outcome of a function execution.
/// </summary>
public interface IOutcome
{
    /// <summary>
    /// The source that has generated the <see cref="IOutcome"/>.
    /// </summary>
    public string Source { get; }
    public OutcomeStatus Status { get; }
    public string? Reason { get; }

    public Kinds Kind { get; }
}
