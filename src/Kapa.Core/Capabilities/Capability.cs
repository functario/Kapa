using Kapa.Abstractions.Capabilities;
using Kapa.Abstractions.Validations;

namespace Kapa.Core.Capabilities;

/// <inheritdoc/>
public record Capability : ICapability
{
    public Capability(string name, string description, IOutcomeMetadata outcomeMetadata)
    {
        Name = name;
        Description = description;
        OutcomeMetadata = outcomeMetadata;
    }

    public Capability(
        string name,
        string description,
        IOutcomeMetadata outcomeMetadata,
        IParameter[] parameters
    )
    {
        Name = name;
        Description = description;
        OutcomeMetadata = outcomeMetadata;
        Parameters = parameters;
    }

    public string Name { get; init; }

    public string Description { get; init; }

    public IReadOnlyCollection<IParameter> Parameters { get; init; } = [];

    public IOutcomeMetadata OutcomeMetadata { get; init; }
}
