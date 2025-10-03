using Kapa.Abstractions.Capabilities;

namespace Kapa.Core.Capabilities;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public sealed class CapabilityAttribute : Attribute
{
    /// <inheritdoc />
    public CapabilityAttribute(string description)
        : this(description, null) { }

    /// <summary>
    /// Initializes a new instance of <see cref="CapabilityAttribute"/>.
    /// </summary>
    /// <param name="description">The description of the action accomplished by the <see cref="ICapability"/>.</param>
    /// <param name="title">The user-friendly name of the <see cref="ICapability"/>.</param>
    public CapabilityAttribute(string description, string? title)
    {
        Description = description;
        Title = title;
    }

    public string? Title { get; }

    public string Description { get; }
}
