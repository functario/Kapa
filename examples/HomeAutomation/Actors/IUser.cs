namespace HomeAutomation.Actors;

public interface IUser : IGeneratedActor
{
    public Home Home { get; set; }
    public Identification Identification { get; set; }

    public static Func<IUser, bool> IsAuthenticated => u => u.Identification != null;

    public static Func<IUser, bool> HasDevices => u => u.Home.Devices.Count != 0;
}
