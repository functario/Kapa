using Kapa.Abstractions.Capabilities;
using Kapa.Abstractions.Rules;
using Kapa.Core.Capabilities;

namespace Kapa.Core.Extensions;

public static class TypeExtensions
{
    public static ICapabilityType ToCapability(this Type capabilityType)
    {
        ArgumentNullException.ThrowIfNull(capabilityType);

        if (!capabilityType.IsDefined(typeof(CapabilityTypeAttribute), inherit: true))
            throw new InvalidOperationException(
                $"Type '{capabilityType.FullName}' does not have '{nameof(CapabilityTypeAttribute)}'."
            );

        var capability = ExtractCapability(capabilityType);
        return new CapabilityType(capability);
    }

    public static ICollection<ICapability> ExtractCapability(Type capabilityType)
    {
        ArgumentNullException.ThrowIfNull(capabilityType);
        var capabilityList = new List<ICapability>();

        var methods = capabilityType.GetMethods(
            System.Reflection.BindingFlags.Instance
                | System.Reflection.BindingFlags.Public
                | System.Reflection.BindingFlags.NonPublic
        );

        foreach (var method in methods)
        {
            var capabilityAttribute = method
                .GetCustomAttributes(typeof(CapabilityAttribute), inherit: true)
                .Cast<CapabilityAttribute>()
                .FirstOrDefault();

            if (capabilityAttribute is not null)
            {
                var capabilities = capabilityAttribute.ToCapability(method);
                var parameters = method.ExtractParameters();
                if (parameters.Count > 0 && capabilities is Capability capability)
                {
                    capability = new Capability(
                        capability.Name,
                        capability.Description,
                        capability.Title
                    )
                    {
                        Parameters = [.. parameters],
                    };

                    capabilityList.Add(capability);
                }
                else
                {
                    capabilityList.Add(capabilities);
                }
            }
        }

        return capabilityList;
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
}
