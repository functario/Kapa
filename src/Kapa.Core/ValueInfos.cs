using Kapa.Abstractions;

namespace Kapa.Core;

public static class ValueInfos
{
    public static IValueInfo None()
    {
        return new ValueInfo(Kinds.NoneKind, "", false, []);
    }
}
