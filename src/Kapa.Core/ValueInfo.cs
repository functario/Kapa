using Kapa.Abstractions;

namespace Kapa.Core;

public record ValueInfo(
    Kinds Kinds,
    string TypeFullPath,
    bool IsGeneric,
    ICollection<IValueInfo> GenericArguments
) : IValueInfo
{ }
