using Kapa.Abstractions;
using Kapa.Abstractions.Results;

namespace Kapa.Core.Results;

public sealed class Outcomes<TOutcome1, TOutcome2> : IOutcome
    where TOutcome1 : IOutcome
    where TOutcome2 : IOutcome
{
    // Use implicit cast operators to create an instance
    private Outcomes(IOutcome current)
    {
        Source = current.Source;
        Status = current.Status;
        Kind = current.Kind;
        Reason = current.Reason;
        Outcome = current;
    }

    public string Source { get; init; }

    public OutcomeStatus Status { get; init; }

    public string? Reason { get; init; }

    public Kinds Kind { get; init; }

    public IOutcome? Outcome { get; init; }

    public static implicit operator Outcomes<TOutcome1, TOutcome2>(TOutcome1 outcome) =>
        new(outcome);

    public static implicit operator Outcomes<TOutcome1, TOutcome2>(TOutcome2 outcome) =>
        new(outcome);
}

public sealed class Outcomes<TOutcome1, TOutcome2, TOutcome3> : IOutcome
    where TOutcome1 : IOutcome
    where TOutcome2 : IOutcome
    where TOutcome3 : IOutcome
{
    private Outcomes(IOutcome current)
    {
        Source = current.Source;
        Status = current.Status;
        Kind = current.Kind;
        Reason = current.Reason;
        Outcome = current;
    }

    public string Source { get; init; }

    public OutcomeStatus Status { get; init; }

    public string? Reason { get; init; }

    public Kinds Kind { get; init; }

    public IOutcome? Outcome { get; init; }

    public static implicit operator Outcomes<TOutcome1, TOutcome2, TOutcome3>(TOutcome1 outcome) =>
        new(outcome);

    public static implicit operator Outcomes<TOutcome1, TOutcome2, TOutcome3>(TOutcome2 outcome) =>
        new(outcome);

    public static implicit operator Outcomes<TOutcome1, TOutcome2, TOutcome3>(TOutcome3 outcome) =>
        new(outcome);
}
