namespace HomeAutomation.Actors;

public interface IUser : IGeneratedActor
{
    public Home Home { get; set; }
    public Identification Identification { get; set; }

    public static Func<IUser, bool> IsAuthenticated => u => u.Identification != null;

    public static Func<IUser, bool> HasThermostat => u => u.Home.Devices.Any(x => x is Thermostat);
    public static Func<IUser, bool> HasLight => u => u.Home.Devices.Any(x => x is Thermostat);
}
