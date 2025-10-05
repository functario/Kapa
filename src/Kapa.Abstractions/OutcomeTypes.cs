namespace Kapa.Abstractions;

[Flags]
public enum OutcomeTypes : long
{
    None = 0,
    Ok = 1L << 0,
    Fail = 1L << 1,
    RulesFail = 1L << 2,
    Union = 1L << 3,
    Generic = 1L << 4,
}
