using System.Reflection;
using Kapa.Abstractions.Capabilities;
using Kapa.Abstractions.Exceptions;
using Kapa.Abstractions.Rules;
using Kapa.Core.Capabilities;

namespace Kapa.Core.Extensions;

public static class TypeExtensions
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="capabilityType"></param>
    /// <returns></returns>
    /// <exception cref="TypeIsNotCapabilityException"></exception>
    public static ICapabilityType ToCapabilityType(this Type capabilityType)
    {
        ArgumentNullException.ThrowIfNull(capabilityType);

        ThrowIfNotCapabilitType(capabilityType);

        var capability = GetCapabilities(capabilityType);

        return new CapabilityType(capability);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="capabilityType"></param>
    /// <returns></returns>
    /// <exception cref="MissingCapabilityException"></exception>
    /// <exception cref="TypeIsNotCapabilityException"></exception>
    public static ICollection<ICapability> GetCapabilities(this Type capabilityType)
    {
        ArgumentNullException.ThrowIfNull(capabilityType);

        ThrowIfNotCapabilitType(capabilityType);

        var capabilities = new List<ICapability>();
        var methods = capabilityType.GetMethods(
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
        );

        foreach (var method in methods)
        {
            if (method.ToCapability() is ICapability capability)
            {
                capabilities.Add(capability);
            }
        }

        ThrowIfMissingCapabilityException(capabilityType, capabilities.Count);

        ThrowIfDuplicatedDescriptions(capabilities, capabilityType);

        return capabilities;
    }

    /// <summary>
    /// Infers the <see cref="ParameterTypes"/> from a CLR type.
    /// </summary>
    /// <param name="paramType">The CLR type to infer <see cref="ParameterTypes"/> from <paramref name="paramType"/>.</param>
    /// <returns>The inferred <see cref="ParameterTypes"/>.</returns>
    public static ParameterTypes InferParamerType(this Type paramType)
    {
        if (paramType is null)
            return ParameterTypes.Null;

        // Handle nullable types
        var underlyingType = Nullable.GetUnderlyingType(paramType) ?? paramType;

        if (underlyingType == typeof(string))
            return ParameterTypes.String;

        if (underlyingType == typeof(bool))
            return ParameterTypes.Boolean;

        // Integer types (JSON integer)
        if (
            underlyingType == typeof(int)
            || underlyingType == typeof(long)
            || underlyingType == typeof(short)
            || underlyingType == typeof(byte)
            || underlyingType == typeof(uint)
            || underlyingType == typeof(ulong)
            || underlyingType == typeof(ushort)
            || underlyingType == typeof(sbyte)
        )
            return ParameterTypes.Integer;

        // Floating-point types (JSON number)
        if (
            underlyingType == typeof(float)
            || underlyingType == typeof(double)
            || underlyingType == typeof(decimal)
        )
            return ParameterTypes.Number;

        // Arrays and collections
        if (
            underlyingType.IsArray
            || (
                underlyingType.IsGenericType
                && (
                    typeof(System.Collections.IEnumerable).IsAssignableFrom(underlyingType)
                    || underlyingType.GetGenericTypeDefinition() == typeof(IEnumerable<>)
                )
            )
        )
            return ParameterTypes.Array;

        // Default to Object for complex types
        return ParameterTypes.Object;
    }

    public static bool IsCapabilityType(this Type type) =>
        type?.IsDefined(typeof(CapabilityTypeAttribute), inherit: true) ?? false;

    internal static IEnumerable<IRule> ToRules(this IReadOnlyCollection<Type> ruleTypes)
    {
        foreach (var type in ruleTypes)
        {
            if (
                typeof(IRule).IsAssignableFrom(type)
                && type.GetConstructor(Type.EmptyTypes) is not null
                && Activator.CreateInstance(type) is IRule instance
            )
            {
                yield return instance;
            }
            else
            {
                throw new InvalidCastException(
                    $"Could not cast type '{type.FullName}' to '{nameof(IRule)}'."
                );
            }
        }
    }

    private static void ThrowIfMissingCapabilityException(Type capabilityType, int count)
    {
        if (count == 0)
        {
            throw new MissingCapabilityException(capabilityType);
        }
    }

    private static void ThrowIfDuplicatedDescriptions(
        ICollection<ICapability> capabilities,
        Type capabilityType
    )
    {
        var duplicateStrings = capabilities
            .GroupBy(x => x.Description) // Group elements by their value
            .Where(g => g.Count() > 1) // Filter groups that have more than one element (duplicates)
            .Select(g => g.Key) // Select the key (the string itself) from these groups
            .ToArray(); // Convert the result to a List<string>

        if (duplicateStrings.Length > 0)
        {
            throw new DuplicateCapabilityDescriptionsException(capabilityType, duplicateStrings);
        }
    }

    private static void ThrowIfNotCapabilitType(this Type type)
    {
        if (!type.IsCapabilityType())
        {
            throw new TypeIsNotCapabilityException(type);
        }
    }
}
