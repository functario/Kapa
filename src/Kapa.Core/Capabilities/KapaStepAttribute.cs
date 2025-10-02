using System.Reflection;
using Kapa.Abstractions.Capabilities;

namespace Kapa.Core.Capabilities;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public sealed class KapaStepAttribute : Attribute
{
    public KapaStepAttribute(string description, string name = "")
    {
        Description = description;
        Name = name;
    }

    public string Name { get; }

    public string Description { get; }

    public IKapaStep ToKapaStep(MethodInfo method)
    {
        ArgumentNullException.ThrowIfNull(method);
        var stepName = string.IsNullOrEmpty(Name) ? method.Name : Name;
        return new KapaStep(stepName, Description);
    }
}
