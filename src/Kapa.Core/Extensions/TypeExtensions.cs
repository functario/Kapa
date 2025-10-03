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
        if (
            GetExceptionIfNotCapabilitType(capabilityType) is TypeIsNotCapabilityException exception
        )
        {
            throw exception;
        }

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
    public static ICollection<ICapability> GetCapabilities(Type capabilityType)
    {
        ArgumentNullException.ThrowIfNull(capabilityType);
        if (
            GetExceptionIfNotCapabilitType(capabilityType) is TypeIsNotCapabilityException exception
        )
        {
            throw exception;
        }

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

        if (capabilities.Count == 0)
        {
            throw new MissingCapabilityException(capabilityType);
        }

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

    private static TypeIsNotCapabilityException? GetExceptionIfNotCapabilitType(this Type type)
    {
        if (type.IsCapabilityType())
        {
            return null;
        }

        return new TypeIsNotCapabilityException(type);
    }
}
