namespace Kapa.Abstractions.Outcomes;

/// <summary>
/// A <see cref="IOutcome"/> with an associated value.
/// </summary>
/// <typeparam name="TKind">The value associated with the <see cref="IOutcome"/>.</typeparam>
public interface ITypedOutcome<TKind> : IOutcome
    where TKind : IKinds
{
    public TKind? Kind { get; }
    public object? Value { get; }
}
