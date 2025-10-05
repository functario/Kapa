namespace Kapa.Abstractions;

[Flags]
public enum Kinds : long
{
    None = 0,
    Ok = 1L << 0,
    Fail = 1L << 1,
    RulesFail = 1L << 2,
    Union = 1L << 3,
    Generic = 1L << 4,
    StringKind = 1L << 5,
    NumberKind = 1L << 6,
    IntegerKind = 1L << 7,
    ObjectKind = 1L << 8,
    ArrayKind = 1L << 9,
    BooleanKind = 1L << 10,
}
