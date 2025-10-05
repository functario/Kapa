namespace Kapa.Abstractions.Validations;

public interface IOutcomeMetadata
{
    /// <summary>
    /// The source that has generated the <see cref="IOutcome"/>.
    /// </summary>
    public string Source { get; }

    /// <summary>
    /// Information about the returned value.
    /// </summary>
    public IValueInfo? ValueInfo { get; }

    public OutcomeTypes OutcomeType { get; }
}
