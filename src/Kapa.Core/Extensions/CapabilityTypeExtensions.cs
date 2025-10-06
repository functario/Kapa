using System.Reflection;
using Kapa.Abstractions.Capabilities;
using Kapa.Abstractions.Exceptions;
using Kapa.Abstractions.Prototypes;
using Kapa.Core.Capabilities;

namespace Kapa.Core.Extensions;

public static class CapabilityTypeExtensions
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

        ThrowIfNotCapabilityType(capabilityType);

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

        ThrowIfNotCapabilityType(capabilityType);

        var capabilities = new List<ICapability>();
        var methods = capabilityType.GetMethods(
            BindingFlags.Instance
                | BindingFlags.Static
                | BindingFlags.Public
                | BindingFlags.NonPublic
        );

        foreach (var method in methods)
        {
            if (method.ToCapability() is ICapability capability)
            {
                capabilities.Add(capability);
            }
        }

        ThrowIfMissingCapabilityException(capabilityType, capabilities.Count);

        return capabilities;
    }

    public static IPrototypeRelations<IHasTrait> GetPrototypeRelations(this Type capabilityType)
    {
        ArgumentNullException.ThrowIfNull(capabilityType);
        ThrowIfNotCapabilityType(capabilityType);

        var method = FindRelationsMethod(capabilityType);
        var attr = GetRelationsAttribute(method);
        var relations = GetRelationsInstance(attr);
        return relations;
    }

    public static bool IsCapabilityType(this Type type) =>
        type?.IsDefined(typeof(CapabilityTypeAttribute), inherit: true) ?? false;

    private static MethodInfo FindRelationsMethod(Type type)
    {
        var method = type.GetMethods(
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
            )
            .FirstOrDefault(m =>
                m.GetCustomAttributes(false)
                    .Any(attr =>
                        attr.GetType().IsGenericType
                        && attr.GetType().GetGenericTypeDefinition()
                            == typeof(Kapa.Core.Capabilities.RelationsAttribute<>)
                    )
            );

        return method
            ?? throw new InvalidOperationException(
                $"No method with RelationsAttribute found on type {type.FullName}."
            );
    }

    private static Attribute GetRelationsAttribute(System.Reflection.MethodInfo method)
    {
        if (
            method
                .GetCustomAttributes(false)
                .FirstOrDefault(a =>
                    a.GetType().IsGenericType
                    && a.GetType().GetGenericTypeDefinition()
                        == typeof(Capabilities.RelationsAttribute<>)
                )
            is not Attribute attr
        )
            throw new InvalidOperationException(
                $"RelationsAttribute not found on method {method.Name} of type {method.DeclaringType?.FullName}."
            );

        return attr;
    }

    private static IPrototypeRelations<IHasTrait> GetRelationsInstance(Attribute attr)
    {
        var relationsProp =
            attr.GetType().GetProperty("Relations") ?? throw new InvalidOperationException(
                $"Relations property not found on RelationsAttribute of type {attr.GetType().FullName}."
            );

        if (relationsProp.GetValue(attr) is not IPrototypeRelations<IHasTrait> relations)
            throw new InvalidCastException(
                $"Relations property is not IPrototypeRelations<IHasTrait> on attribute {attr.GetType().FullName}."
            );

        return relations;
    }

    private static void ThrowIfMissingCapabilityException(Type capabilityType, int count)
    {
        if (count == 0)
        {
            throw new MissingCapabilityException(capabilityType);
        }
    }

    private static void ThrowIfNotCapabilityType(this Type type)
    {
        if (!type.IsCapabilityType())
        {
            throw new TypeIsNotCapabilityException(
                type,
                $"The attribute '{nameof(CapabilityTypeAttribute)}' does not decorate the class."
            );
        }
    }
}
