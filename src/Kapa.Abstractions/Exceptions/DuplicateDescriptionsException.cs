namespace Kapa.Abstractions.Exceptions;

public sealed class DuplicateDescriptionsException : Exception
{
    private static string GetMessage(Type type, Type descriptionSource, params string[] duplicates)
    {
        var duplicatesMessage = string.Join(", ", duplicates);
        return $"'{type.FullName}' contains one or more duplicated descriptions of '{descriptionSource.FullName}'."
            + $"Duplicates: '{duplicatesMessage}'.";
    }

    public DuplicateDescriptionsException(
        Type type,
        Type descriptionSource,
        params string[] duplicates
    )
        : base(GetMessage(type, descriptionSource, duplicates)) { }

    public DuplicateDescriptionsException() { }

    public DuplicateDescriptionsException(string message)
        : base(message) { }

    public DuplicateDescriptionsException(string message, Exception innerException)
        : base(message, innerException) { }
}
