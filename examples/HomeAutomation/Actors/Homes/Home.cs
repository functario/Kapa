namespace HomeAutomation.Actors.Homes;

public sealed class Home : IGeneratedActor
{
    public Home(ICollection<IDevice> devices)
    {
        Devices = devices;
    }

    public ICollection<IDevice> Devices { get; init; }

    public TDevice? GetDevice<TDevice>(string deviceName)
    {
        if (Devices.Where(d => d.Name == deviceName).FirstOrDefault() is TDevice device)
        {
            return device;
        }

        return default;
    }
}
