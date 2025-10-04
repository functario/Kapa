using System.Reflection;
using Kapa.Abstractions.Capabilities;
using Kapa.Core.Capabilities;

namespace Kapa.Core.Extensions;

internal static class MethodInfoExtensions
{
    public static CapabilityAttribute? GetCapabilityAttribute(this MethodInfo method) =>
        method
            ?.GetCustomAttributes(typeof(CapabilityAttribute), inherit: true)
            .Cast<CapabilityAttribute>()
            .FirstOrDefault();

    public static ICapability? ToCapability(this MethodInfo method)
    {
        ArgumentNullException.ThrowIfNull(method);

        var capabilityAttribute = method.GetCapabilityAttribute();
        if (capabilityAttribute is null)
        {
            return null;
        }

        var parameters = GetParameters(method);
        if (parameters.Count > 0)
        {
            return new Capability(method.Name, capabilityAttribute.Description, [.. parameters]);
        }

        return new Capability(method.Name, capabilityAttribute.Description);
    }

    public static ICollection<IParameter> GetParameters(this MethodInfo method)
    {
        ArgumentNullException.ThrowIfNull(method);
        var capabilityParameters = new List<IParameter>();
        var methodParameters = method.GetParameters();
        foreach (var param in methodParameters)
        {
            var paramAttr = param
                .GetCustomAttributes(typeof(ParameterAttribute), inherit: true)
                .Cast<ParameterAttribute>()
                .FirstOrDefault();

            if (paramAttr is not null)
            {
                capabilityParameters.Add(paramAttr.ToParameter(param));
            }
        }

        return capabilityParameters;
    }
}
