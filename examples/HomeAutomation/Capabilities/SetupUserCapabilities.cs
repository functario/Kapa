using System.Reflection;
using Kapa.Core.Extensions;
using Kapa.Core.Factories;

namespace HomeAutomation.Capabilities;

[CapabilityType]
public sealed class SetupUserCapabilities
{
    public SetupUserCapabilities() { }

    [Capability($"Setup the {nameof(User)}.")]
    [Relations<SetupRelations>]
    public Ok<IUser> SetupUser(IUser user)
    {
        ArgumentNullException.ThrowIfNull(user);
        IDevice[] devices =
        [
            new Thermostat(Guid.NewGuid(), "Thermostat00", "ThermostatModel"),
            new Thermostat(Guid.NewGuid(), "Thermostat01", "ThermostatModel"),
            new Light(Guid.NewGuid(), "Light00", "LightModel"),
        ];

        user.Home = new Home(devices);

        user.Identification.IsAuthenticated = true;
        user.Identification.Token = Token.CreateDummy();

        return TypedOutcomes.Ok(MethodInfo.GetCurrentMethod(), user);
    }
}

public sealed class SetupRelations : IRelations<IGeneratedActor>
{
    public ICollection<IEffect<IGeneratedActor>> Mutations =>
        [
            // For example: Mutation does not implement the same EffectPredicate but has the same Id
            EffectFactory.Create<User>(
                u => u.Home.Devices.Any(x => x is Thermostat),
                IUser.HasThermostatId,
                "Has Thermostat"
            ),
            IUser.HasLight.ToEffect(),
            IUser.IsAuthenticated.ToEffect(),
        ];

    public ICollection<IEffect<IGeneratedActor>> Requirements => [];
}
