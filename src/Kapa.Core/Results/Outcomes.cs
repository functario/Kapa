using Kapa.Abstractions;
using Kapa.Abstractions.Results;

namespace Kapa.Core.Results;

public sealed class Outcomes<TOutcome1, TOutcome2> : IOutcome
    where TOutcome1 : IOutcome
    where TOutcome2 : IOutcome
{
    // Use implicit cast operators to create an instance
    private Outcomes(IOutcome activeResult)
    {
        Source = activeResult.Source;
        Status = activeResult.Status;
        Kind = activeResult.Kind;
        Reason = activeResult.Reason;
        MyOutcome = activeResult;
    }

    public string Source { get; init; }

    public OutcomeStatus Status { get; init; }

    public string? Reason { get; init; }

    public Kinds Kind { get; init; }

    public IOutcome? MyOutcome { get; init; }
    public object? Value => MyOutcome;

    public static implicit operator Outcomes<TOutcome1, TOutcome2>(TOutcome1 result) => new(result);

    public static implicit operator Outcomes<TOutcome1, TOutcome2>(TOutcome2 result) => new(result);
}

public sealed class Outcomes<TOutcome1, TOutcome2, TOutcome3> : IOutcome
    where TOutcome1 : IOutcome
    where TOutcome2 : IOutcome
    where TOutcome3 : IOutcome
{
    private Outcomes(IOutcome activeResult)
    {
        Source = activeResult.Source;
        Status = activeResult.Status;
        Kind = activeResult.Kind;
        Reason = activeResult.Reason;
        MyOutcome = activeResult;
    }

    public string Source { get; init; }

    public OutcomeStatus Status { get; init; }

    public string? Reason { get; init; }

    public Kinds Kind { get; init; }

    public IOutcome? MyOutcome { get; init; }
    public object? Value => MyOutcome;

    public static implicit operator Outcomes<TOutcome1, TOutcome2, TOutcome3>(TOutcome1 result) =>
        new(result);

    public static implicit operator Outcomes<TOutcome1, TOutcome2, TOutcome3>(TOutcome2 result) =>
        new(result);

    public static implicit operator Outcomes<TOutcome1, TOutcome2, TOutcome3>(TOutcome3 result) =>
        new(result);
}
