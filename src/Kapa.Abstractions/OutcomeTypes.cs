namespace Kapa.Abstractions;

[Flags]
public enum OutcomeTypes
{
    None = 0,
    Ok = 1,
    Fail = 2,
    RulesFail = 4,
    Union = 8,
    Generic = 16,
}
