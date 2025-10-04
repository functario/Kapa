using Kapa.Abstractions.Capabilities;

namespace Kapa.Core.Extensions;

public static class ParameterTypeExtensions
{
    /// <summary>
    /// Infers the <see cref="ParameterTypes"/> from a CLR type.
    /// </summary>
    /// <param name="paramType">The CLR type to infer <see cref="ParameterTypes"/> from <paramref name="paramType"/>.</param>
    /// <returns>The inferred <see cref="ParameterTypes"/>.</returns>
    public static ParameterTypes InferParamerType(this Type paramType)
    {
        ArgumentNullException.ThrowIfNull(paramType);

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
}
