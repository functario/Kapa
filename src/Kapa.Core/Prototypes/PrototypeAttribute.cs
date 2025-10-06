using Kapa.Abstractions.Prototypes;

namespace Kapa.Core.Prototypes;

/// <summary>
/// Indicates that the class is a <see cref="IPrototype"/>.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public sealed class PrototypeAttribute : Attribute
{
    public PrototypeAttribute(string description)
    {
        Description = description;
    }

    public string Description { get; }
}
