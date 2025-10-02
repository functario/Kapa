namespace Kapa.Abstractions.Outcomes;

public interface IOutcome
{
    public string CapabilityName { get; }
    public OutcomeStatus Status { get; }
    public string? Reason { get; }
}
