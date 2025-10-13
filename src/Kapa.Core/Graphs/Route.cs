using Kapa.Abstractions.Graphs;

namespace Kapa.Core.Graphs;

/// <summary>
/// Represents a valid path through the dependency <see cref="Graph"/>.
/// </summary>
/// <param name="Index">An arbritary index to reference the <see cref="IRoute"/>
/// solenely relevant in the current <see cref="IGraph"/> instance context.</param>
/// <param name="Edges">The ordered sequence of <see cref="Edge"/> composing this <see cref="Route"/>.</param>
public sealed record Route(int Index, IReadOnlyList<IEdge> Edges) : IRoute
{
    /// <summary>
    /// Determines whether this <see cref="IRoute"/> is equal to another.
    /// Comparison is based on <see cref="Index"/> property.
    /// </summary>
    /// <param name="other">The <see cref="IRoute"/> to compare with.</param>
    /// <returns>True if both routes have the same <see cref="Index"/>, false otherwise.</returns>
    public bool Equals(IRoute? other)
    {
        if (other is null)
            return false;
        if (ReferenceEquals(this, other))
            return true;
        return Index == other.Index;
    }

    /// <summary>
    /// Returns the hash code for this <see cref="IRoute"/>.
    /// Hash code is computed from <see cref="Index"/> property.
    /// </summary>
    /// <returns>A hash code for the current route.</returns>
    public override int GetHashCode()
    {
        return Index.GetHashCode();
    }
}
