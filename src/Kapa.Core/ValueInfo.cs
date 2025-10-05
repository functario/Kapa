using Kapa.Abstractions;

namespace Kapa.Core;

public record ValueInfo(
    Kinds Kinds,
    string FullName,
    bool IsGeneric,
    ICollection<IValueInfo> GenericArguments
) : IValueInfo
{ }
