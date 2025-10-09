using System.Reflection;
using Kapa.Abstractions.Rules;
using Kapa.Abstractions.Validations;

namespace HomeAutomation.Rules.ThermostatRules;

public sealed class ThermostatSetpointRule : IRule
{
    private const double MinSetpoint = 5;
    private const double MaxSetpoint = 30;

    public string Name =>
        $"The {nameof(Thermostat)} {nameof(Thermostat.Setpoint)} range"
        + $" is limited between '{MinSetpoint}' and '{MaxSetpoint}' (Celsius).";

    public Type TypeOfT => typeof(Thermostat);

    public IOutcome Validate<T>(T subject)
    {
        if (subject is not Thermostat thermostat)
            throw new InvalidCastException();

        if (thermostat.Setpoint < MinSetpoint || thermostat.Setpoint > MaxSetpoint)
        {
            return TypedOutcomes.Fail(
                MethodBase.GetCurrentMethod(),
                new InvalidOperationException(
                    $"{nameof(Thermostat.Setpoint)} '{thermostat.Setpoint}' is not in allowed values "
                        + $"'{MinSetpoint}' and '{MaxSetpoint}' (Celsius)."
                )
            );
        }

        return TypedOutcomes.Ok(MethodBase.GetCurrentMethod());
    }
}
