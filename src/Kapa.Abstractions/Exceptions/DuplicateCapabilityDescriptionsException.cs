using Kapa.Abstractions.Capabilities;

namespace Kapa.Abstractions.Exceptions;

public sealed class DuplicateCapabilityDescriptionsException : Exception
{
    private static string GetMessage(Type type, params string[] duplicates)
    {
        var duplica = string.Join(", ", duplicates);
        return $"{type.FullName} contains one or more duplicated {nameof(ICapability)}"
            + $" descriptions: '{duplica}'.";
    }

    public DuplicateCapabilityDescriptionsException(Type type, params string[] duplicates)
        : base(GetMessage(type, duplicates)) { }

    public DuplicateCapabilityDescriptionsException() { }

    public DuplicateCapabilityDescriptionsException(string message)
        : base(message) { }

    public DuplicateCapabilityDescriptionsException(string message, Exception innerException)
        : base(message, innerException) { }
}
