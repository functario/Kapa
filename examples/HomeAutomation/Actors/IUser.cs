namespace HomeAutomation.Actors;

public interface IUser : IGeneratedActor, IActor
{
    public Home Home { get; set; }
    public Identification Identification { get; set; }

    // For the purpose of showing shared Id examples.
    public const string HasThermostatId = $"{nameof(IUser)}.{nameof(HasThermostat)}";

    [EffectPredicate(
        $"{nameof(IUser)}.{nameof(IsAuthenticated)}",
        $"The {nameof(IUser)} is authenticated."
    )]
    public static Predicate<IUser> IsAuthenticated => u => u.Identification != null;

    [EffectPredicate(
        HasThermostatId,
        $"The {nameof(IUser)}'s {nameof(Home)} has at least 1 {nameof(Thermostat)}."
    )]
    public static Predicate<IUser> HasThermostat => u => u.Home.Devices.Any(x => x is Thermostat);

    [EffectPredicate(
        $"{nameof(IUser)}.{nameof(HasLight)}",
        $"The {nameof(IUser)}'s {nameof(Home)} has at least 1 {nameof(Light)}."
    )]
    public static Predicate<IUser> HasLight => u => u.Home.Devices.Any(x => x is Light);
}
