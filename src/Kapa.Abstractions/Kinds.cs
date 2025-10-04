using System.Diagnostics;

namespace Kapa.Abstractions;

/// <inheritdoc/>
public sealed class Kinds : IKinds
{
    private Kinds(string name)
    {
        Name = name;
    }

    public string Name { get; }

    public static IKinds StringKind => new Kinds(nameof(IKinds.StringKind));

    public static IKinds GetNumberKind => new Kinds(nameof(IKinds.NumberKind));

    public static IKinds GetIntegerKind => new Kinds(nameof(IKinds.IntegerKind));

    public static IKinds GetObjectKind => new Kinds(nameof(IKinds.ObjectKind));

    public static IKinds GetArrayKind => new Kinds(nameof(IKinds.ArrayKind));

    public static IKinds GetBooleanKind => new Kinds(nameof(IKinds.BooleanKind));

    public override string ToString() => Name;

    IKinds IKinds.StringKind => throw new UnreachableException();

    IKinds IKinds.NumberKind => throw new UnreachableException();

    IKinds IKinds.IntegerKind => throw new UnreachableException();

    IKinds IKinds.ObjectKind => throw new UnreachableException();

    IKinds IKinds.ArrayKind => throw new UnreachableException();

    IKinds IKinds.BooleanKind => throw new UnreachableException();
}
