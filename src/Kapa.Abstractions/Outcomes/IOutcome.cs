namespace Kapa.Abstractions.Outcomes;

public interface IOutcome
{
    public string KapaStepName { get; }
    public OutcomeStatus Status { get; }
    public string? Reason { get; }
}
