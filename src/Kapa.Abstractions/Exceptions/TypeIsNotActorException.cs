using Kapa.Abstractions.Actors;

namespace Kapa.Abstractions.Exceptions;

public sealed class TypeIsNotActorException : Exception
{
    private static string GetMessage(Type type, string hint)
    {
        var hintMessage = string.IsNullOrWhiteSpace(hint) ? string.Empty : $" Hint: '{hint}'.";
        return $"{type.FullName} is not {nameof(IActor)}.{hintMessage}";
    }

    public TypeIsNotActorException(Type type, string hint)
        : base(GetMessage(type, hint)) { }

    public TypeIsNotActorException() { }

    public TypeIsNotActorException(string message)
        : base(message) { }

    public TypeIsNotActorException(string message, Exception innerException)
        : base(message, innerException) { }
}
