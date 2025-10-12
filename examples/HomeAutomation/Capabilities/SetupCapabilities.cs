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
            // Mutation does not implement the same EffectPredicate but has the same Id
            EffectFactory.Create<User>(
                u => u.Home.Devices.Any(x => x is Thermostat),
                IUser.HasThermostatId,
                "Has Thermostat"
            ),
            IUser.HasLight.ToEffect(),
        ];

    public ICollection<IEffect<IGeneratedActor>> Requirements => [];
}
