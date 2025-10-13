using Kapa.Abstractions.Actors;
using Kapa.Abstractions.Capabilities;
using Kapa.Abstractions.Graphs;

namespace Kapa.Core.Graphs;

/// <summary>
/// A <see cref="Node"/> in the dependency graph
/// with <paramref name="Mutations"/> and <paramref name="Requirements"/>.
/// </summary>
/// <param name="Capability">The <see cref="ICapability"/> associated to this node .</param>
/// <param name="Mutations">The <see cref="IEffect{TGeneratedActor}"/> provided by this <see cref="ICapability"/>.</param>
/// <param name="Requirements">The <see cref="IEffect{TGeneratedActor}"/> that must be satisfied for this <see cref="ICapability"/>.</param>
public sealed record Node(
    ICapability Capability,
    ICollection<IEffect<IGeneratedActor>> Mutations,
    ICollection<IEffect<IGeneratedActor>> Requirements
) : INode
{
    /// <summary>
    /// Determines whether this <see cref="INode"/> is equal to another.
    /// Comparison is based on the <see cref="ICapability.OutcomeMetadata"/> Source property.
    /// </summary>
    /// <param name="other">The <see cref="INode"/> to compare with.</param>
    /// <returns>True if both nodes have the same <see cref="ICapability"/> source, false otherwise.</returns>
    public bool Equals(INode? other)
    {
        if (other is null)
            return false;
        if (ReferenceEquals(this, other))
            return true;
        return Capability.OutcomeMetadata.Source == other.Capability.OutcomeMetadata.Source;
    }

    /// <summary>
    /// Returns the hash code for this <see cref="INode"/>.
    /// Hash code is computed from the <see cref="Capability"/> OutcomeMetadata Source property.
    /// </summary>
    /// <returns>A hash code for the current node.</returns>
    public override int GetHashCode()
    {
        return Capability.OutcomeMetadata.Source.GetHashCode(StringComparison.Ordinal);
    }
}
