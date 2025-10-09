using Kapa.Core.Prototypes;

namespace HomeAutomation.Prototypes.Homes;

[Prototype($"Information about the user {nameof(Home)}.")]
public sealed record Home([State($"All the {nameof(User)} devices")] ICollection<IDevice> Devices)
    : IGeneratedPrototype
{
    public TDevice? GetDevice<TDevice>(string deviceName)
    {
        if (Devices.Where(d => d.Name == deviceName).FirstOrDefault() is TDevice device)
        {
            return device;
        }

        return default;
    }
}
