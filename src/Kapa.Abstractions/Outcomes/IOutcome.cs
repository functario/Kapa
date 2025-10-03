namespace Kapa.Abstractions.Outcomes;

public interface IOutcome
{
    public string Source { get; }
    public OutcomeStatus Status { get; }
    public string? Reason { get; }
}
