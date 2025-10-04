using Kapa.Abstractions;
using Kapa.Abstractions.Outcomes;

namespace Kapa.Core.Outcomes;

public sealed class Results<TOutcome1, TOutcome2> : IOutcome
    where TOutcome1 : IOutcome
    where TOutcome2 : IOutcome
{
    // Use implicit cast operators to create an instance
    private Results(IOutcome activeResult)
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

    public static implicit operator Results<TOutcome1, TOutcome2>(TOutcome1 result) => new(result);

    public static implicit operator Results<TOutcome1, TOutcome2>(TOutcome2 result) => new(result);
}

public sealed class Results<TOutcome1, TOutcome2, TOutcome3> : IOutcome
    where TOutcome1 : IOutcome
    where TOutcome2 : IOutcome
    where TOutcome3 : IOutcome
{
    private Results(IOutcome activeResult)
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

    public static implicit operator Results<TOutcome1, TOutcome2, TOutcome3>(TOutcome1 result) =>
        new(result);

    public static implicit operator Results<TOutcome1, TOutcome2, TOutcome3>(TOutcome2 result) =>
        new(result);

    public static implicit operator Results<TOutcome1, TOutcome2, TOutcome3>(TOutcome3 result) =>
        new(result);
}
