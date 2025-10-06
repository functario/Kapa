using Kapa.Abstractions.Capabilities;
using Kapa.Abstractions.Prototypes;
using Kapa.Abstractions.Validations;

namespace Kapa.Core.Capabilities;

/// <inheritdoc/>
public record Capability : ICapability
{
    public Capability(
        string name,
        string description,
        IOutcomeMetadata outcomeMetadata,
        IParameter[] parameters
    )
        : this(name, description, outcomeMetadata, null, parameters) { }

    public Capability(
        string name,
        string description,
        IOutcomeMetadata outcomeMetadata,
        IPrototypeRelations<IHasTrait>? relations
    )
        : this(name, description, outcomeMetadata, relations, []) { }

    public Capability(string name, string description, IOutcomeMetadata outcomeMetadata)
        : this(name, description, outcomeMetadata, null, []) { }

    public Capability(
        string name,
        string description,
        IOutcomeMetadata outcomeMetadata,
        IPrototypeRelations<IHasTrait>? relations,
        IParameter[] parameters
    )
    {
        Name = name;
        Description = description;
        OutcomeMetadata = outcomeMetadata;
        Relations = relations;
        Parameters = parameters;
    }

    public string Name { get; init; }

    public string Description { get; init; }

    public IReadOnlyCollection<IParameter> Parameters { get; init; } = [];

    public IOutcomeMetadata OutcomeMetadata { get; init; }

    public IPrototypeRelations<IHasTrait>? Relations { get; init; }
}
