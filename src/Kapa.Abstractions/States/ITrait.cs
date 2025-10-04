using Kapa.Abstractions.Capabilities;

namespace Kapa.Abstractions.States;

/// <summary>
/// Property that can be mutated by <see cref="ICapability"/>.
/// </summary>
public interface ITrait
{
    string Name { get; }
    string Description { get; }
    public IReadOnlyCollection<IParameter> Parameters { get; }
}
