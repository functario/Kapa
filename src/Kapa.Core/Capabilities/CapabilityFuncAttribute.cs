using Kapa.Abstractions.Capabilities;

namespace Kapa.Core.Capabilities;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public sealed class CapabilityFuncAttribute : Attribute, ICapabilityFunc
{
    public CapabilityFuncAttribute(string description, string name = "")
    {
        Description = description;
        Name = name;
    }

    public string Name { get; }

    public string Description { get; }
}
