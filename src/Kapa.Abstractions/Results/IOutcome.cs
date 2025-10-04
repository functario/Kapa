namespace Kapa.Abstractions.Results;

/// <summary>
/// The outcome of a function execution.
/// </summary>
public interface IOutcome
{
    public string Source { get; }
    public OutcomeStatus Status { get; }
    public string? Reason { get; }

    public Kinds Kind { get; }
}
