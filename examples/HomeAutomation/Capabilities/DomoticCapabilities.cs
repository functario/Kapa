using System.Reflection;
using HomeAutomation.Rules.ThermostatRules;

namespace HomeAutomation.Capabilities;

[CapabilityType]
public sealed class DomoticCapabilities
{
    private readonly IUser _user;

    public DomoticCapabilities(IUser user)
    {
        _user = user;
    }

    [Capability($"Change the {nameof(Thermostat)} {nameof(Thermostat.Setpoint)}.")]
    public async Task<Outcomes<Ok<IUser>, Fail<string>>> SetThermostatSetpoint(
        [Parameter($"The {nameof(Thermostat)} name.", typeof(ThermostatSetpointRule))]
            string thermostatName,
        [Parameter($"The setpoint to apply to the {nameof(Thermostat)}.")] double setpoint
    )
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(thermostatName, nameof(thermostatName));

        if (
            _user.Home?.Devices.Where(x => x.Name == thermostatName).FirstOrDefault()
            is not Thermostat thermostat
        )
        {
            return TypedOutcomes.Fail<string>(
                MethodInfo.GetCurrentMethod(),
                $"Thermostat '{nameof(thermostatName)}' was not found."
            );
        }

        // Busy task
        await Task.Delay(10);
        thermostat.Setpoint = setpoint;

        return TypedOutcomes.Ok(MethodInfo.GetCurrentMethod(), _user);
    }
}
