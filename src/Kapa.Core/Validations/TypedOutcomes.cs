using System.Reflection;
using Kapa.Abstractions.Validations;

namespace Kapa.Core.Validations;

public static class TypedOutcomes
{
    public static Ok<TValue> Ok<TValue>(string source, TValue value) => new(source, value);

    public static Ok<TValue> Ok<TValue>(string source, TValue value, string reason) =>
        new(source, value, reason);

    /// <summary>
    /// Initializes a new instance of <see cref="Validations.Ok{TValue}"/>
    /// </summary>
    /// <typeparam name="TValue">The type of <see cref="IOutcome"/> returned value.</typeparam>
    /// <param name="source">The method returning the <see cref="IOutcome"/>.</param>
    /// <param name="value">The <see cref="IOutcome"/> returned value.</param>
    /// <returns><see cref="Validations.Ok{TValue}"/></returns>
    public static Ok<TValue> Ok<TValue>(MethodBase? source, TValue value) => new(source, value);

    public static Ok<TValue> Ok<TValue>(MethodBase? source, TValue value, string reason) =>
        new(source, value, reason);

    public static Fail<TValue> Fail<TValue>(string source, TValue value) => new(source, value);

    public static Fail<TValue> Fail<TValue>(string source, TValue value, string reason) =>
        new(source, value, reason);

    public static Fail<TValue> Fail<TValue>(MethodBase? source, TValue value) => new(source, value);

    public static Fail<TValue> Fail<TValue>(MethodBase? source, TValue value, string reason) =>
        new(source, value, reason);

    public static RulesFail<TValue> RulesFail<TValue>(string source, TValue value) =>
        new(source, value);

    public static RulesFail<TValue> RulesFail<TValue>(string source, TValue value, string reason) =>
        new(source, value, reason);

    public static RulesFail<TValue> RulesFail<TValue>(MethodBase? source, TValue value) =>
        new(source, value);

    public static RulesFail<TValue> RulesFail<TValue>(
        MethodBase? source,
        TValue value,
        string reason
    ) => new(source, value, reason);

    public static Ok Ok(string source) => new(source);

    public static Ok Ok(string source, string reason) => new(source, reason);

    public static Ok Ok(MethodBase? source) => new(source);

    public static Ok Ok(MethodBase? source, string reason) => new(source, reason);

    public static Fail Fail(string source) => new(source);

    public static Fail Fail(string source, string reason) => new(source, reason);

    public static Fail Fail(MethodBase? source) => new(source);

    public static Fail Fail(MethodBase? source, string reason) => new(source, reason);

    public static RulesFail RulesFail(string source) => new(source);

    public static RulesFail RulesFail(string source, string reason) => new(source, reason);

    public static RulesFail RulesFail(MethodBase? source) => new(source);

    public static RulesFail RulesFail(MethodBase? source, string reason) => new(source, reason);
}
