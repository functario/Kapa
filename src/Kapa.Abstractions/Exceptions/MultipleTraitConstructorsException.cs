using Kapa.Abstractions.Prototypes;

namespace Kapa.Abstractions.Exceptions;

public sealed class MultipleTraitConstructorsException : Exception
{
    private static string GetMessage(Type type, string hint)
    {
        var hintMessage = string.IsNullOrWhiteSpace(hint) ? string.Empty : $" Hint: '{hint}'.";
        return $"Multiple constructors have been found on {nameof(ITrait)} '{type.FullName}'."
            + $"There should only be one applicable constructor.{hintMessage}";
    }

    public MultipleTraitConstructorsException(Type type, string hint)
        : base(GetMessage(type, hint)) { }

    public MultipleTraitConstructorsException() { }

    public MultipleTraitConstructorsException(string message)
        : base(message) { }

    public MultipleTraitConstructorsException(string message, Exception innerException)
        : base(message, innerException) { }
}
