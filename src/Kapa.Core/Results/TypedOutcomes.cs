namespace Kapa.Core.Results;

public static class TypedOutcomes
{
    public static Ok<TValue> Ok<TValue>(string source, TValue value) => new(source, value);

    public static Fail<TValue> Fail<TValue>(string source, TValue value) => new(source, value);

    public static RulesFail<TValue> RulesFail<TValue>(string source, TValue value) =>
        new(source, value);

    public static Ok<TValue> Ok<TValue>(string source, TValue value, string reason) =>
        new(source, value, reason);

    public static Fail<TValue> Fail<TValue>(string source, TValue value, string reason) =>
        new(source, value, reason);

    public static RulesFail<TValue> RulesFail<TValue>(string source, TValue value, string reason) =>
        new(source, value, reason);

    public static Ok Ok(string source) => new(source);

    public static Fail Fail(string source) => new(source);

    public static RulesFail RulesFail(string source) => new(source);

    public static Ok Ok(string source, string reason) => new(source, reason);

    public static Fail Fail(string source, string reason) => new(source, reason);

    public static RulesFail RulesFail(string source, string reason) => new(source, reason);
}
