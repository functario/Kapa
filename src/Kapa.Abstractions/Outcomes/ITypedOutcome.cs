namespace Kapa.Abstractions.Outcomes;

public interface ITypedOutcome<TValue> : IOutcome
{
    public TValue? Value { get; }
}
