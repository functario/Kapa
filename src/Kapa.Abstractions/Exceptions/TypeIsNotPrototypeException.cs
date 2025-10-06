using Kapa.Abstractions.Prototypes;

namespace Kapa.Abstractions.Exceptions;

public sealed class TypeIsNotPrototypeException : Exception
{
    private static string GetMessage(Type type, string hint)
    {
        var hintMessage = string.IsNullOrWhiteSpace(hint) ? string.Empty : $" Hint: '{hint}'.";
        return $"{type.FullName} is not {nameof(IPrototype)}.{hintMessage}";
    }

    public TypeIsNotPrototypeException(Type type, string hint)
        : base(GetMessage(type, hint)) { }

    public TypeIsNotPrototypeException() { }

    public TypeIsNotPrototypeException(string message)
        : base(message) { }

    public TypeIsNotPrototypeException(string message, Exception innerException)
        : base(message, innerException) { }
}
