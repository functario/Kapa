using Kapa.Abstractions.Capabilities;
using Kapa.Abstractions.Rules;
using Kapa.Core.Capabilities;

namespace Kapa.Core.Extensions;

public static class TypeExtensions
{
    public static IKapability ToKapability(this Type kapabilityType)
    {
        ArgumentNullException.ThrowIfNull(kapabilityType);

        if (!kapabilityType.IsDefined(typeof(KapabilityAttribute), inherit: true))
            throw new InvalidOperationException(
                $"Type '{kapabilityType.FullName}' does not have '{nameof(KapabilityAttribute)}'."
            );

        var kapaSteps = ExtractKapaSteps(kapabilityType);
        return new Kapability(kapaSteps);
    }

    public static ICollection<IKapaStep> ExtractKapaSteps(Type kapabilityType)
    {
        ArgumentNullException.ThrowIfNull(kapabilityType);
        var kapaSteps = new List<IKapaStep>();

        var methods = kapabilityType.GetMethods(
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
                var step = stepAttr.ToKapaStep(method);
                var kapaParams = method.ExtractKapaParams();
                if (kapaParams.Count > 0 && step is KapaStep stepRecord)
                {
                    stepRecord = new KapaStep(
                        stepRecord.Name,
                        stepRecord.Description,
                        stepRecord.Title
                    )
                    {
                        Parameters = [.. kapaParams],
                    };

                    kapaSteps.Add(stepRecord);
                }
                else
                {
                    kapaSteps.Add(step);
                }
            }
        }

        return kapaSteps;
    }

    /// <summary>
    /// Infers the KapaParamTypes from a CLR type.
    /// </summary>
    /// <param name="paramType">The CLR type to infer KapaParamTypes from.</param>
    /// <returns>The inferred KapaParamTypes.</returns>
    public static KapaParamTypes InferKapaParamType(this Type paramType)
    {
        if (paramType is null)
            return KapaParamTypes.Null;

        // Handle nullable types
        var underlyingType = Nullable.GetUnderlyingType(paramType) ?? paramType;

        if (underlyingType == typeof(string))
            return KapaParamTypes.String;

        if (underlyingType == typeof(bool))
            return KapaParamTypes.Boolean;

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
            return KapaParamTypes.Integer;

        // Floating-point types (JSON number)
        if (
            underlyingType == typeof(float)
            || underlyingType == typeof(double)
            || underlyingType == typeof(decimal)
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
            else
            {
                throw new InvalidCastException(
                    $"Could not cast type '{type.FullName}' to '{nameof(IRule)}'."
                );
            }
        }
    }
}
