using Kapa.Abstractions.Actors;

namespace Kapa.Core.Actors;

/// <summary>
/// Indicates that the class is a <see cref="IActor"/>.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public sealed class ActorAttribute : Attribute
{
    public ActorAttribute(string description)
    {
        Description = description;
    }

    public string Description { get; }
}
