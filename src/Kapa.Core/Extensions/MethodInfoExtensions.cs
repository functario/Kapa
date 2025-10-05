using System.Reflection;
using Kapa.Abstractions.Capabilities;
using Kapa.Core.Capabilities;
using Kapa.Core.Validations;

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
        var returnValueType = method.ReturnParameter.ParameterType;
        var valueInfo = returnValueType.GetValueInfo();
        var outcomeTypes = returnValueType.InferOutcomeTypes();
        var source = InferSourceName(capabilityAttribute, method);

        var outcomeMetadata = new OutcomeMetadata(source, valueInfo, outcomeTypes);

        if (parameters.Count > 0)
        {
            return new Capability(
                method.Name,
                capabilityAttribute.Description,
                outcomeMetadata,
                [.. parameters]
            );
        }

        return new Capability(method.Name, capabilityAttribute.Description, outcomeMetadata);
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

    private static string InferSourceName(
        CapabilityAttribute capabilityAttribute,
        MethodInfo method
    )
    {
        if (capabilityAttribute.Source is not null)
            return capabilityAttribute.Source;

        if (method.DeclaringType is null)
            return method.Name;

        var typeIdentifier = GetTypeIdentifier(method.DeclaringType);

        // Handle generic method
        var methodName = method.Name;
        if (method.IsGenericMethod)
        {
            var genericArgs = method.GetGenericArguments();
            var genericArgNames = string.Join(",", genericArgs.Select(g => g.Name));
            methodName += $"<{genericArgNames}>";
        }

        // Build parameter signature
        var parameters = method.GetParameters();
        if (parameters.Length == 0)
        {
            return $"{typeIdentifier}.{methodName}()";
        }

        var paramSignature = string.Join(
            ", ",
            parameters.Select(p => GetTypeIdentifier(p.ParameterType))
        );
        return $"{typeIdentifier}.{methodName}({paramSignature})";
    }

    private static string GetTypeIdentifier(Type type)
    {
        // Handle generic types
        if (type.IsGenericType)
        {
            var genericTypeDef = type.GetGenericTypeDefinition();
            var genericArgs = type.GetGenericArguments();
            var genericArgIdentifiers = string.Join(", ", genericArgs.Select(GetTypeIdentifier));

            var baseName = genericTypeDef.FullName ?? genericTypeDef.Name;
            var tickIndex = baseName.IndexOf('`', StringComparison.Ordinal);
            if (tickIndex > 0)
            {
                baseName = baseName.Substring(0, tickIndex);
            }

            return $"{baseName}<{genericArgIdentifiers}>";
        }

        return type.FullName ?? type.Name;
    }
}
