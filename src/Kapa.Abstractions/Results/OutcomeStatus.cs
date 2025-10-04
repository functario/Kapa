namespace Kapa.Abstractions.Results;

/// <summary>
/// The <see cref="IOutcome"/> status.
/// </summary>
public enum OutcomeStatus
{
    NotExecuted = 0,
    Ok,
    Fail,
    RulesFail,
}
