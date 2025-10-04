namespace Kapa.Abstractions;

/// <inheritdoc/>
public class Kinds
{
    private Kinds()
    {
        Name = "Undefined";
    }

    private Kinds(string name)
    {
        Name = name;
    }

    public string Name { get; }

    public override string ToString() => Name;

    public static Kinds NoneKind => new(nameof(Kinds.NoneKind));

    public static Kinds StringKind => new(nameof(Kinds.StringKind));

    public static Kinds NumberKind => new(nameof(Kinds.NumberKind));

    public static Kinds IntegerKind => new(nameof(Kinds.IntegerKind));

    public static Kinds ObjectKind => new(nameof(Kinds.ObjectKind));

    public static Kinds ArrayKind => new(nameof(Kinds.ArrayKind));

    public static Kinds BooleanKind => new(nameof(Kinds.BooleanKind));

    public static Kinds[] GetKinds()
    {
        return [StringKind, NumberKind, IntegerKind, ObjectKind, ArrayKind, BooleanKind];
    }
}
