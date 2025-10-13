using Kapa.Abstractions.Actors;
using Kapa.Abstractions.Capabilities;
using Kapa.Abstractions.Graphs;

namespace Kapa.Core.Graphs;

/// <summary>
/// Represents a directed edge from a <see cref="ICapability"/>
/// providing <see cref="IRelations{TGeneratedActor}.Mutations"/> to resolve
/// an another <see cref="ICapability"/>'s <see cref="IRelations{TGeneratedActor}.Requirements"/>.
/// </summary>
/// <param name="FromCapacity">The <see cref="ICapability"/> that provides <see cref="IRelations{TGeneratedActor}.Mutations"/>.</param>
/// <param name="ToCapacity">The <see cref="ICapability"/> that has <see cref="IRelations{TGeneratedActor}.Requirements"/>.</param>
/// <param name="ResolvingMutations">The <see cref="IEffect{TGeneratedActor}"/> from
/// <paramref name="FromCapacity"/> that help satisfy <paramref name="ToCapacity"/>'s
/// <param name="Index">An arbritary index to reference the <see cref="IEdge"/> solenely relevant in the current <see cref="IGraph"/> instance context.</param>
/// <see cref="IRelations{TGeneratedActor}.Requirements"/>.</param>
public sealed record Edge(
    INode FromCapacity,
    INode ToCapacity,
    ICollection<IEffect<IGeneratedActor>> ResolvingMutations,
    int Index
) : IEdge
{
    /// <summary>
    /// Determines whether this <see cref="IEdge"/> is equal to another.
    /// Comparison is based on <see cref="FromCapacity"/>, <see cref="ToCapacity"/>,
    /// <see cref="Index"/>, and <see cref="ResolvingMutations"/> collections (compared by <see cref="IEffect{TGeneratedActor}.Id"/>).
    /// </summary>
    /// <param name="other">The <see cref="IEdge"/> to compare with.</param>
    /// <returns>True if both edges have the same from/to nodes, index, and resolving mutations, false otherwise.</returns>
    public bool Equals(IEdge? other)
    {
        if (other is null)
            return false;
        if (ReferenceEquals(this, other))
            return true;

        if (!FromCapacity.Equals(other.FromCapacity))
            return false;

        if (!ToCapacity.Equals(other.ToCapacity))
            return false;

        if (Index != other.Index)
            return false;

        // Compare ResolvingMutations collections by their IDs
        if (ResolvingMutations.Count != other.ResolvingMutations.Count)
            return false;

        return ResolvingMutations.All(rm => other.ResolvingMutations.Any(orm => rm.Equals(orm)));
    }

    /// <summary>
    /// Returns the hash code for this <see cref="IEdge"/>.
    /// Hash code is computed from <see cref="FromCapacity"/>, <see cref="ToCapacity"/>,
    /// <see cref="Index"/>, and all <see cref="ResolvingMutations"/>.
    /// </summary>
    /// <returns>A hash code for the current edge.</returns>
    public override int GetHashCode()
    {
        var hash = new HashCode();
        hash.Add(FromCapacity);
        hash.Add(ToCapacity);
        hash.Add(Index);
        foreach (var mutation in ResolvingMutations.OrderBy(m => m.Id))
        {
            hash.Add(mutation);
        }
        return hash.ToHashCode();
    }
}
