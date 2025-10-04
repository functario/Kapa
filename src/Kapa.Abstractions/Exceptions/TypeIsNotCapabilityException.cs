using Kapa.Abstractions.Capabilities;

namespace Kapa.Abstractions.Exceptions;

public sealed class TypeIsNotCapabilityException : Exception
{
    private static string GetMessage(Type type, string hint)
    {
        var hintMessage = string.IsNullOrWhiteSpace(hint) ? string.Empty : $" Hint: '{hint}'.";
        return $"'{type.FullName}' is not '{nameof(ICapabilityType)}'.{hintMessage}";
    }

    public TypeIsNotCapabilityException(Type type, string hint)
        : base(GetMessage(type, hint)) { }

    public TypeIsNotCapabilityException() { }

    public TypeIsNotCapabilityException(string message)
        : base(message) { }

    public TypeIsNotCapabilityException(string message, Exception innerException)
        : base(message, innerException) { }
}
