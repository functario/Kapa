namespace Kapa.Abstractions;

public static class TypeExtensions
{
    /// <summary>
    /// Infers the <see cref="Kinds"/> from a CLR type.
    /// </summary>
    /// <param name="paramType">The CLR type to infer <see cref="Kinds"/> from <paramref name="paramType"/>.</param>
    /// <returns>The inferred <see cref="Kinds"/>.</returns>
    public static Kinds InferKind(this Type paramType)
    {
        ArgumentNullException.ThrowIfNull(paramType);

        // Handle nullable types
        var underlyingType = Nullable.GetUnderlyingType(paramType) ?? paramType;

        if (underlyingType == typeof(string))
            return Kinds.StringKind;

        if (underlyingType == typeof(bool))
            return Kinds.BooleanKind;

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

        // Default to Object for complex types
        return Kinds.ObjectKind;
    }
}
