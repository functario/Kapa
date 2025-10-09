using System.Reflection;

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
