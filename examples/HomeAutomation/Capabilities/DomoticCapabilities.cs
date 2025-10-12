using System.Reflection;
using HomeAutomation.Rules.ThermostatRules;
using Kapa.Core.Extensions;

namespace HomeAutomation.Capabilities;

[CapabilityType]
public sealed class DomoticCapabilities
{
    private readonly IUser _user;

    public DomoticCapabilities(IUser user)
    {
        _user = user;
    }

    [Capability($"Change the {nameof(Light)} {nameof(Light.IsOn)} state.")]
    [Relations<SetLightRelations>]
    public async Task<Outcomes<Ok<IUser>, Fail<string>>> SetLightIsOn(
        [Parameter($"The {nameof(Light)} name.")] string lightName,
        [Parameter($"The {nameof(Light.IsOn)} state to apply to the {nameof(Light)}.")] bool isOn
    )
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(lightName, nameof(lightName));

        if (_user.Home?.Devices.Where(x => x.Name == lightName).FirstOrDefault() is not Light light)
        {
            return TypedOutcomes.Fail<string>(
                MethodInfo.GetCurrentMethod(),
                $"{nameof(Light)} '{nameof(lightName)}' was not found."
            );
        }

        // Busy task
        await Task.Delay(10);
        light.IsOn = isOn;

        return TypedOutcomes.Ok(MethodInfo.GetCurrentMethod(), _user);
    }

    [Capability($"Change the {nameof(Thermostat)} {nameof(Thermostat.Setpoint)}.")]
    [Relations<SetThermostatSetpointRelations>]
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
                $"{nameof(Thermostat)} '{nameof(thermostatName)}' was not found."
            );
        }

        // Busy task
        await Task.Delay(10);
        thermostat.Setpoint = setpoint;

        return TypedOutcomes.Ok(MethodInfo.GetCurrentMethod(), _user);
    }

    [Capability($"Add a {nameof(Thermostat)} to the {nameof(Home)}.")]
    [Relations<AddThermostatRelations>]
    public async Task<Outcomes<Ok<IUser>, Fail<string>>> AddThermostat(
        [Parameter($"The {nameof(Thermostat)} name.")] string thermostatName,
        [Parameter($"The {nameof(Thermostat)} model.")] string model
    )
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(thermostatName, nameof(thermostatName));
        ArgumentException.ThrowIfNullOrWhiteSpace(model, nameof(model));

        var thermostat = new Thermostat(Guid.NewGuid(), thermostatName, model);
        _user.Home?.Devices.Add(thermostat);

        // Busy task
        await Task.Delay(10);

        return TypedOutcomes.Ok(MethodInfo.GetCurrentMethod(), _user);
    }

    [Capability($"Add a {nameof(Light)} to the {nameof(Home)}.")]
    [Relations<AddLightRelations>]
    public async Task<Outcomes<Ok<IUser>, Fail<string>>> AddLight(
        [Parameter($"The {nameof(Light)} name.")] string thermostatName,
        [Parameter($"The {nameof(Light)} model.")] string model
    )
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(thermostatName, nameof(thermostatName));
        ArgumentException.ThrowIfNullOrWhiteSpace(model, nameof(model));

        var thermostat = new Light(Guid.NewGuid(), thermostatName, model);
        _user.Home?.Devices.Add(thermostat);

        // Busy task
        await Task.Delay(10);

        return TypedOutcomes.Ok(MethodInfo.GetCurrentMethod(), _user);
    }
}

public sealed class SetThermostatSetpointRelations : IRelations<IGeneratedActor>
{
    public ICollection<IEffect<IGeneratedActor>> Mutations => [];

    public ICollection<IEffect<IGeneratedActor>> Requirements =>
        [IUser.IsAuthenticated.ToEffect(), IUser.HasThermostat.ToEffect()];
}

public sealed class SetLightRelations : IRelations<IGeneratedActor>
{
    public ICollection<IEffect<IGeneratedActor>> Mutations => [];

    public ICollection<IEffect<IGeneratedActor>> Requirements =>
        [IUser.IsAuthenticated.ToEffect(), IUser.HasLight.ToEffect()];
}

public sealed class AddThermostatRelations : IRelations<IGeneratedActor>
{
    public ICollection<IEffect<IGeneratedActor>> Mutations => [IUser.HasThermostat.ToEffect()];

    public ICollection<IEffect<IGeneratedActor>> Requirements => [IUser.IsAuthenticated.ToEffect()];
}

public sealed class AddLightRelations : IRelations<IGeneratedActor>
{
    public ICollection<IEffect<IGeneratedActor>> Mutations => [IUser.HasLight.ToEffect()];

    public ICollection<IEffect<IGeneratedActor>> Requirements => [IUser.IsAuthenticated.ToEffect()];
}
