using Kapa.Abstractions.Actors;
using Kapa.Abstractions.Capabilities;
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
        IRelations<IGeneratedActor>? relations
    )
        : this(name, description, outcomeMetadata, relations, []) { }

    public Capability(string name, string description, IOutcomeMetadata outcomeMetadata)
        : this(name, description, outcomeMetadata, null, []) { }

    public Capability(
        string name,
        string description,
        IOutcomeMetadata outcomeMetadata,
        IRelations<IGeneratedActor>? relations,
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

    public IRelations<IGeneratedActor>? Relations { get; init; }

    /// <summary>
    /// Determines whether this <see cref="ICapability"/> is equal to another.
    /// Comparison is based on <see cref="IOutcomeMetadata.Source"/> property from <see cref="OutcomeMetadata"/>.
    /// </summary>
    /// <param name="other">The <see cref="ICapability"/> to compare with.</param>
    /// <returns>True if both capabilities have the same <see cref="IOutcomeMetadata.Source"/>, false otherwise.</returns>
    public bool Equals(ICapability? other)
    {
        if (other is null)
            return false;
        if (ReferenceEquals(this, other))
            return true;
        return OutcomeMetadata.Source == other.OutcomeMetadata.Source;
    }

    /// <summary>
    /// Returns the hash code for this <see cref="ICapability"/>.
    /// Hash code is computed from <see cref="IOutcomeMetadata.Source"/> property.
    /// </summary>
    /// <returns>A hash code for the current capability.</returns>
    public override int GetHashCode()
    {
        return OutcomeMetadata.Source.GetHashCode(StringComparison.Ordinal);
    }
}
