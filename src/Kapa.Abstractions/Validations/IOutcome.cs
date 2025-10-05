namespace Kapa.Abstractions.Validations;

/// <summary>
/// The outcome of a function execution.
/// </summary>
public interface IOutcome : IOutcomeMetadata
{
    public OutcomeStatus Status { get; }
    public string? Reason { get; }
}
