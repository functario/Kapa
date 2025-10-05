using System.Reflection;

namespace Kapa.Core.Extensions;

internal static class MethodBaseExtensions
{
    public static string InferSourceName(this MethodBase method)
    {
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
                baseName = baseName[..tickIndex];
            }

            return $"{baseName}<{genericArgIdentifiers}>";
        }

        return type.FullName ?? type.Name;
    }
}
