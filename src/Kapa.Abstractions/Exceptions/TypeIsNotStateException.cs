using Kapa.Abstractions.States;

namespace Kapa.Abstractions.Exceptions;

public sealed class TypeIsNotStateException : Exception
{
    private static string GetMessage(Type type, string hint)
    {
        var hintMessage = string.IsNullOrWhiteSpace(hint) ? string.Empty : $" Hint: '{hint}'.";
        return $"{type.FullName} is not {nameof(IState)}.{hintMessage}";
    }

    public TypeIsNotStateException(Type type, string hint)
        : base(GetMessage(type, hint)) { }

    public TypeIsNotStateException() { }

    public TypeIsNotStateException(string message)
        : base(message) { }

    public TypeIsNotStateException(string message, Exception innerException)
        : base(message, innerException) { }
}
