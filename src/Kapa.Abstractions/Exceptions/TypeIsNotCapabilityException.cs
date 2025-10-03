using Kapa.Abstractions.Capabilities;

namespace Kapa.Abstractions.Exceptions;

public sealed class TypeIsNotCapabilityException : Exception
{
    private static string GetMessage(Type type)
    {
        return $"{type.FullName} is not {nameof(ICapabilityType)}.";
    }

    public TypeIsNotCapabilityException(Type type)
        : base(GetMessage(type)) { }

    public TypeIsNotCapabilityException() { }

    public TypeIsNotCapabilityException(string message)
        : base(message) { }

    public TypeIsNotCapabilityException(string message, Exception innerException)
        : base(message, innerException) { }
}
