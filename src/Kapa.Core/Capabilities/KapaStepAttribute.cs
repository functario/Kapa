using System.Reflection;
using Kapa.Abstractions.Capabilities;

namespace Kapa.Core.Capabilities;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public sealed class KapaStepAttribute : Attribute
{
    /// <inheritdoc />
    public KapaStepAttribute(string description)
        : this(description, null) { }

    /// <summary>
    /// Initializes a new instance of <see cref="KapaStepAttribute"/>.
    /// </summary>
    /// <param name="description">The description of the action accomplished by the <see cref="IKapaStep"/>.</param>
    /// <param name="title">The user-friendly name of the <see cref="IKapaStep"/>.</param>
    public KapaStepAttribute(string description, string? title)
    {
        Description = description;
        Title = title;
    }

    public string? Title { get; }

    public string Description { get; }

    public IKapaStep ToKapaStep(MethodInfo method)
    {
        ArgumentNullException.ThrowIfNull(method);
        return new KapaStep(method.Name, Description, Title);
    }
}
