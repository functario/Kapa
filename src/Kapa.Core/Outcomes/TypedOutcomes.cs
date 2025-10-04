using Kapa.Abstractions.Outcomes;

namespace Kapa.Core.Outcomes;

public static class TypedOutcomes
{
    public static Ok<TValue> Ok<TValue>(string source, TValue value) =>
        new(source, OutcomeStatus.Ok, value);

    public static Fail<TValue> Fail<TValue>(string source, TValue value) =>
        new(source, OutcomeStatus.Fail, value);

    public static Fail<TValue> RulesFail<TValue>(string source, TValue value) =>
        new(source, OutcomeStatus.RulesFail, value);

    public static IOutcome Ok(string source) => new Outcome(source, OutcomeStatus.Ok);

    public static IOutcome Fail(string source) => new Outcome(source, OutcomeStatus.Fail);
}
