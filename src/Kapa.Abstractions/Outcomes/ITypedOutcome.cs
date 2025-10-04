namespace Kapa.Abstractions.Outcomes;

/// <summary>
/// A <see cref="IOutcome"/> with an associated value.
/// </summary>
/// <typeparam name="TValue">The value associated with the <see cref="IOutcome"/>.</typeparam>
public interface ITypedOutcome<TValue> : IOutcome
{
    public TValue? Value { get; }
}
