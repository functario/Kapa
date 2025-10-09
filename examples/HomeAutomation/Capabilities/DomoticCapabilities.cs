using System.Reflection;
using HomeAutomation.Prototypes;
using HomeAutomation.Prototypes.Homes;
using Kapa.Core.Capabilities;
using Kapa.Core.Validations;

namespace HomeAutomation.Capabilities;

public sealed class DomoticCapabilities
{
    private readonly IUser _user;

    public DomoticCapabilities(IUser user)
    {
        _user = user;
    }

    [Capability($"Change the {nameof(Thermostat)} {nameof(Thermostat.Setpoint)}.")]
    public async Task<Outcomes<Ok<IUser>, Fail<string>>> SetThermostatSetpoint(
        string thermostatName,
        double setpoint
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
