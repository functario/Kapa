using System.Reflection;
using Kapa.Core.Extensions;
using Kapa.Core.Factories;

namespace HomeAutomation.Capabilities;

[CapabilityType]
public sealed class SetupCapabilities
{
    private readonly IUser _user;

    public SetupCapabilities(IUser user)
    {
        _user = user;
    }

    [Capability($"Setup the {nameof(User)}.")]
    [Relations<SetupRelations>]
    public Ok<IUser> Setup()
    {
        IDevice[] devices =
        [
            new Thermostat(Guid.NewGuid(), "Thermostat00", "ThermostatModel"),
            new Thermostat(Guid.NewGuid(), "Thermostat01", "ThermostatModel"),
            new Light(Guid.NewGuid(), "Light00", "LightModel"),
        ];

        _user.Home = new Home(devices);

        return TypedOutcomes.Ok(MethodInfo.GetCurrentMethod(), _user);
    }
}

public sealed class SetupRelations : IRelations<IGeneratedActor>
{
    public ICollection<IEffect<IGeneratedActor>> Mutations =>
        [
            EffectFactory.Create<User>(
                u => u.Home.Devices.Any(x => x is Thermostat),
                nameof(IUser.HasThermostat),
                "Has Thermostat"
            ),
            //IUser.HasThermostat.ToMutation("Has Thermostat"),
            IUser.HasLight.ToEffect(nameof(IUser.HasLight), "Has Light"),
        ];

    public ICollection<IEffect<IGeneratedActor>> Requirements => [];
}
