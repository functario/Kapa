namespace Kapa.Abstractions.Capabilities;

/// <summary>
/// A type holding <see cref="ICapability"/>.
/// </summary>
public interface ICapabilityType
{
    public IReadOnlyCollection<ICapability> Capabilities { get; }
}
