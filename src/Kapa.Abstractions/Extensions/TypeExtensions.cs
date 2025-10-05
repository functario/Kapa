using Kapa.Abstractions.Validations;

namespace Kapa.Abstractions.Extensions;

public static class TypeExtensions
{
    private static readonly Dictionary<string, Kinds> s_typeFullNameToKind = new()
    {
        { typeof(string).FullName!, Kinds.StringKind },
        { typeof(bool).FullName!, Kinds.BooleanKind },
        { typeof(int).FullName!, Kinds.IntegerKind },
        { typeof(long).FullName!, Kinds.IntegerKind },
        { typeof(short).FullName!, Kinds.IntegerKind },
        { typeof(byte).FullName!, Kinds.IntegerKind },
        { typeof(uint).FullName!, Kinds.IntegerKind },
        { typeof(ulong).FullName!, Kinds.IntegerKind },
        { typeof(ushort).FullName!, Kinds.IntegerKind },
        { typeof(sbyte).FullName!, Kinds.IntegerKind },
        { typeof(float).FullName!, Kinds.NumberKind },
        { typeof(double).FullName!, Kinds.NumberKind },
        { typeof(decimal).FullName!, Kinds.NumberKind },
    };

    /// <summary>
    /// Infers the <see cref="Kinds"/> from a CLR type.
    /// </summary>
    /// <param name="type">The CLR type to infer <see cref="Kinds"/> from <paramref name="type"/>.</param>
    /// <returns>The inferred <see cref="Kinds"/>.</returns>
    public static Kinds InferKind(this Type type)
    {
        ArgumentNullException.ThrowIfNull(type);

        var underlyingType = Nullable.GetUnderlyingType(type) ?? type;
        if (s_typeFullNameToKind.TryGetValue(underlyingType.FullName!, out var kind))
            return kind;

        if (underlyingType == typeof(string))
            return Kinds.StringKind;

        if (underlyingType == typeof(bool))
            return Kinds.BooleanKind;

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
            return Kinds.IntegerKind;

        // Floating-point types (JSON number)
        if (
            underlyingType == typeof(float)
            || underlyingType == typeof(double)
            || underlyingType == typeof(decimal)
        )
            return Kinds.NumberKind;

        // Arrays and collections
        if (
            underlyingType.IsArray
            || underlyingType.IsGenericType
                && (
                    typeof(System.Collections.IEnumerable).IsAssignableFrom(underlyingType)
                    || underlyingType.GetGenericTypeDefinition() == typeof(IEnumerable<>)
                )
        )
            return Kinds.ArrayKind;

        // Outcomes
        if (underlyingType.IsAssignableTo(typeof(IOutcome)))
            return underlyingType.InferOutcomeKind();

        // Default to Object for complex types
        return Kinds.ObjectKind;
    }

    public static Kinds InferOutcomeKind(this Type outcomeType)
    {
        ArgumentNullException.ThrowIfNull(outcomeType);
        if (!outcomeType.IsAssignableTo(typeof(IOutcome)))
        {
            return outcomeType.InferKind();
        }

        // Handle generic types
        if (outcomeType.IsGenericType)
        {
            var genericDef = outcomeType.GetGenericTypeDefinition();

            // Check if the concrete type implements the generic outcome interfaces
            var interfaces = outcomeType.GetInterfaces();
            foreach (var iface in interfaces)
            {
                if (iface.IsGenericType)
                {
                    var ifaceGenericDef = iface.GetGenericTypeDefinition();
                    if (ifaceGenericDef == typeof(IOk<>))
                        return Kinds.Ok | Kinds.Generic;
                    if (ifaceGenericDef == typeof(IFail<>))
                        return Kinds.Fail | Kinds.Generic;
                    if (ifaceGenericDef == typeof(IRulesFail<>))
                        return Kinds.RulesFail | Kinds.Generic;
                }
            }

            // Check if the type implements IOutcomes
            if (typeof(IOutcomes).IsAssignableFrom(outcomeType))
            {
                var args = outcomeType.GetGenericArguments();
                var result = Kinds.Generic;
                foreach (var arg in args)
                {
                    if (typeof(IOutcome).IsAssignableFrom(arg))
                    {
                        result |= arg.InferOutcomeKind();
                    }
                }
                return result;
            }
        }

        // Handle non-generic types (after checking generic to avoid false positives)
        if (outcomeType.IsAssignableTo(typeof(IOk)))
            return Kinds.Ok;
        if (outcomeType.IsAssignableTo(typeof(IFail)))
            return Kinds.Fail;
        if (outcomeType.IsAssignableTo(typeof(IRulesFail)))
            return Kinds.RulesFail;

        return Kinds.None;
    }
}
