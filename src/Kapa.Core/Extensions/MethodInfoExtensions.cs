using System.Reflection;
using Kapa.Abstractions.Capabilities;
using Kapa.Core.Capabilities;

namespace Kapa.Core.Extensions;

public static class MethodInfoExtensions
{
    public static ICollection<IParameter> ExtractParameters(this MethodInfo method)
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
