using Kapa.Abstractions.Prototypes;

namespace Kapa.Core.Prototypes;

/// <summary>
/// Indicates that the property is a <see cref="IState"/>.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public sealed class StateAttribute : Attribute
{
    public StateAttribute(string description)
    {
        Description = description;
    }

    public string Description { get; }
}
