using Kapa.Abstractions.Actors;

namespace Kapa.Core.Actors;

/// <summary>
/// Represents an effect (mutation or requirement) in the capability graph.
/// Effects are compared by their unique <see cref="Id"/> (case-insensitive).
/// </summary>
/// <typeparam name="TGeneratedActor">The type of <see cref="IGeneratedActor"/> this effect applies to.</typeparam>
/// <param name="Id">Unique identifier for this effect.</param>
/// <param name="Description">Human-readable description of what this effect represents.</param>
/// <param name="Predicate">Function to test if an <see cref="IGeneratedActor"/> satisfies this effect.</param>
public sealed record Effect<TGeneratedActor>(
    string Id,
    string Description,
    Func<IGeneratedActor, bool> Predicate
) : IEffect<TGeneratedActor>
    where TGeneratedActor : IGeneratedActor
{
    /// <summary>
    /// Checks if this effect is equal to another by comparing <see cref="Id"/> values (case-insensitive).
    /// </summary>
    /// <param name="other">The <see cref="IEffect{IGeneratedActor}"/> to compare with.</param>
    /// <returns>True if both effects have the same <see cref="Id"/>, false otherwise.</returns>
    public bool AreEqual(IEffect<IGeneratedActor> other) =>
        Id.Equals(other?.Id, StringComparison.OrdinalIgnoreCase);

    /// <summary>
    /// Determines whether this <see cref="IEffect{TGeneratedActor}"/> is equal to another.
    /// Comparison is based on <see cref="Id"/> property (case-insensitive).
    /// </summary>
    /// <param name="other">The <see cref="IEffect{IGeneratedActor}"/> to compare with.</param>
    /// <returns>True if both effects have the same <see cref="Id"/>, false otherwise.</returns>
    public bool Equals(IEffect<IGeneratedActor>? other)
    {
        if (other is null)
            return false;
        if (ReferenceEquals(this, other))
            return true;
        return Id.Equals(other.Id, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Returns the hash code for this <see cref="IEffect{TGeneratedActor}"/>.
    /// Hash code is computed from <see cref="Id"/> property (case-insensitive).
    /// </summary>
    /// <returns>A hash code for the current effect.</returns>
    public override int GetHashCode()
    {
        return Id.GetHashCode(StringComparison.OrdinalIgnoreCase);
    }
}
