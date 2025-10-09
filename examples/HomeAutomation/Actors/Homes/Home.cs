using Kapa.Core.Actors;

namespace HomeAutomation.Actors.Homes;

[Actor($"Information about the user {nameof(Home)}.")]
public sealed class Home : IGeneratedActor
{
    public Home(ICollection<IDevice> devices)
    {
        Devices = devices;
    }

    [State($"All the {nameof(User)} devices")]
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
