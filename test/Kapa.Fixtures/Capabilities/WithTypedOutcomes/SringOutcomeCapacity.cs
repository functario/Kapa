namespace Kapa.Fixtures.Capabilities.WithTypedOutcomes;

[CapabilityType]
public sealed class SringOutcomeCapacity
{
    public SringOutcomeCapacity(IOutcome? outcomeToReturn = null)
    {
        OutcomeToReturn =
            outcomeToReturn ?? TypedOutcomes.Ok(nameof(SringOutcomeCapacity), "value");
        ;
    }

    public IOutcome OutcomeToReturn { get; }

    [Capability(nameof(Handle))]
    public IOutcome Handle()
    {
        return OutcomeToReturn;
    }
}
