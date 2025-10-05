using Kapa.Abstractions.Validations;

namespace Kapa.Abstractions.Capabilities;

public interface ICapability
{
    public string Name { get; }
    public string Description { get; }
    public IReadOnlyCollection<IParameter> Parameters { get; }
    public IOutcomeMetadata OutcomeMetadata { get; }
}
