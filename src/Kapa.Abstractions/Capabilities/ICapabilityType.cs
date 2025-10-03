namespace Kapa.Abstractions.Capabilities;

public interface ICapabilityType
{
    public IReadOnlyCollection<ICapability> Capabilities { get; }
}
