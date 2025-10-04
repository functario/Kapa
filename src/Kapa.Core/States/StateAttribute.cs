using Kapa.Abstractions.States;

namespace Kapa.Core.States;

/// <summary>
/// Indicates that the class is a <see cref="IState"/>.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public sealed class StateAttribute : Attribute
{
    public StateAttribute(string description)
    {
        Description = description;
    }

    public string Description { get; }
}
