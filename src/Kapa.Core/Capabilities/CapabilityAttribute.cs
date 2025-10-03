using Kapa.Abstractions.Capabilities;

namespace Kapa.Core.Capabilities;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public sealed class CapabilityAttribute : Attribute
{
    /// <summary>
    /// Initializes a new instance of <see cref="CapabilityAttribute"/>.
    /// </summary>
    /// <param name="description">The description of the action accomplished by the <see cref="ICapability"/>.</param>
    public CapabilityAttribute(string description)
    {
        Description = description;
    }

    public string Description { get; }
}
