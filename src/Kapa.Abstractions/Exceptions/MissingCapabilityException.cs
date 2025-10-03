using Kapa.Abstractions.Capabilities;

namespace Kapa.Abstractions.Exceptions;

public sealed class MissingCapabilityException : Exception
{
    private static string GetMessage(Type type)
    {
        return $"{type.FullName} is {nameof(ICapabilityType)} but is missing {nameof(ICapability)}.";
    }

    public MissingCapabilityException(Type type)
        : base(GetMessage(type)) { }

    public MissingCapabilityException() { }

    public MissingCapabilityException(string message)
        : base(message) { }

    public MissingCapabilityException(string message, Exception innerException)
        : base(message, innerException) { }
}
