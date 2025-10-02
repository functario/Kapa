using Kapa.Abstractions.Capabilities;
using Kapa.Abstractions.Rules;
using Kapa.Core.Capabilities;

namespace Kapa.Core.Extensions;

public static class TypeExtensions
{
    public static IKapability ToKapability(this Type type)
    {
        ArgumentNullException.ThrowIfNull(type);

        if (!type.IsDefined(typeof(KapabilityAttribute), inherit: true))
            throw new InvalidOperationException(
                $"Type '{type.FullName}' does not have '{nameof(KapabilityAttribute)}'."
            );

        var kapaSteps = new List<IKapaStep>();
        var methods = type.GetMethods(
            System.Reflection.BindingFlags.Instance
                | System.Reflection.BindingFlags.Public
                | System.Reflection.BindingFlags.NonPublic
        );

        foreach (var method in methods)
        {
            var stepAttr = method
                .GetCustomAttributes(typeof(KapaStepAttribute), inherit: true)
                .Cast<KapaStepAttribute>()
                .FirstOrDefault();

            if (stepAttr is not null)
            {
                kapaSteps.Add(stepAttr.ToKapaStep(method));
            }
        }

        return new Kapability(kapaSteps);
    }

    /// <summary>
    /// Infers the KapaParamTypes from a CLR type.
    /// </summary>
    /// <param name="type">The CLR type to infer KapaParamTypes from.</param>
    /// <returns>The inferred KapaParamTypes.</returns>
    public static KapaParamTypes InferKapaParamType(this Type type)
    {
        if (type == null)
            return KapaParamTypes.Null;

        // Handle nullable types
        var underlyingType = Nullable.GetUnderlyingType(type) ?? type;

        if (underlyingType == typeof(string))
            return KapaParamTypes.String;

        if (underlyingType == typeof(bool))
            return KapaParamTypes.Boolean;

        // Numeric types
        if (
            underlyingType == typeof(int)
            || underlyingType == typeof(long)
            || underlyingType == typeof(float)
            || underlyingType == typeof(double)
            || underlyingType == typeof(decimal)
            || underlyingType == typeof(short)
            || underlyingType == typeof(byte)
            || underlyingType == typeof(uint)
            || underlyingType == typeof(ulong)
            || underlyingType == typeof(ushort)
            || underlyingType == typeof(sbyte)
        )
            return KapaParamTypes.Number;

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
            return KapaParamTypes.Array;

        // Default to Object for complex types
        return KapaParamTypes.Object;
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

            throw new InvalidCastException($"Could not cast type '{type}' to '{nameof(IRule)}'.");
        }
    }
}
