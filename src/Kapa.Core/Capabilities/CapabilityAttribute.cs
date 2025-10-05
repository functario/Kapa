using System.Reflection;
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
        : this(description, null) { }

    /// <summary>
    /// Initializes a new instance of <see cref="CapabilityAttribute"/>.
    /// </summary>
    /// <param name="description">The description of the action accomplished by the <see cref="ICapability"/>.</param>
    /// <param name="source">The source of the <see cref="ICapability"/>.
    /// If none is provide when resolving a <see cref="ICapability"/>,
    /// the <see cref="Type.FullName"/> and <see cref="MethodInfo"/>'s signature will be used.
    /// </param>
    private CapabilityAttribute(string description, string? source)
    {
        Description = description;
        Source = source;
    }

    public string Description { get; }

    /// <summary>
    /// The source of the <see cref="ICapability"/> composed of
    /// the <see cref="Type.FullName"/> and <see cref="MethodInfo"/>'s signature.
    /// </summary>
    public string? Source { get; }
}
