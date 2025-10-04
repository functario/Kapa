using Kapa.Abstractions;

namespace Kapa.Core.Extensions;

public static class ParameterTypeExtensions
{
    /// <summary>
    /// Infers the <see cref="SupportedKinds"/> from a CLR type.
    /// </summary>
    /// <param name="paramType">The CLR type to infer <see cref="SupportedKinds"/> from <paramref name="paramType"/>.</param>
    /// <returns>The inferred <see cref="SupportedKinds"/>.</returns>
    public static SupportedKinds InferKind(this Type paramType)
    {
        ArgumentNullException.ThrowIfNull(paramType);

        // Handle nullable types
        var underlyingType = Nullable.GetUnderlyingType(paramType) ?? paramType;

        if (underlyingType == typeof(string))
            return SupportedKinds.String;

        if (underlyingType == typeof(bool))
            return SupportedKinds.Boolean;

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
            return SupportedKinds.Integer;

        // Floating-point types (JSON number)
        if (
            underlyingType == typeof(float)
            || underlyingType == typeof(double)
            || underlyingType == typeof(decimal)
        )
            return SupportedKinds.Number;

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
            return SupportedKinds.Array;

        // Default to Object for complex types
        return SupportedKinds.Object;
    }
}
