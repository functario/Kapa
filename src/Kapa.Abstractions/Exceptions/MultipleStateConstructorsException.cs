using Kapa.Abstractions.Prototypes;

namespace Kapa.Abstractions.Exceptions;

public sealed class MultipleStateConstructorsException : Exception
{
    private static string GetMessage(Type type, string hint)
    {
        var hintMessage = string.IsNullOrWhiteSpace(hint) ? string.Empty : $" Hint: '{hint}'.";
        return $"Multiple constructors have been found on {nameof(IState)} '{type.FullName}'."
            + $"There should only be one applicable constructor.{hintMessage}";
    }

    public MultipleStateConstructorsException(Type type, string hint)
        : base(GetMessage(type, hint)) { }

    public MultipleStateConstructorsException() { }

    public MultipleStateConstructorsException(string message)
        : base(message) { }

    public MultipleStateConstructorsException(string message, Exception innerException)
        : base(message, innerException) { }
}
