using Kapa.Abstractions.Capabilities;

namespace Kapa.Abstractions.Exceptions;

public sealed class TypeIsNotCapabilityException : Exception
{
    private static string GetMessage(Type type, string hint)
    {
        return $"{type.FullName} is not {nameof(ICapabilityType)}. Hint: '{hint}'";
    }

    public TypeIsNotCapabilityException(Type type, string hint)
        : base(GetMessage(type, hint)) { }

    public TypeIsNotCapabilityException() { }

    public TypeIsNotCapabilityException(string message)
        : base(message) { }

    public TypeIsNotCapabilityException(string message, Exception innerException)
        : base(message, innerException) { }
}
