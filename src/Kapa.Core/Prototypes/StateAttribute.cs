using Kapa.Abstractions.Prototypes;

namespace Kapa.Core.Prototypes;

/// <summary>
/// Indicates that the property is a <see cref="IState"/>.
/// </summary>
/// <remarks>
/// Record positional constructor properties are not supported.
/// Only explict properties are supported.
/// </remarks>
[AttributeUsage(AttributeTargets.Property)]
public sealed class StateAttribute : Attribute
{
    public StateAttribute(string description)
    {
        Description = description;
    }

    public string Description { get; }
}
