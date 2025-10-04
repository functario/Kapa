using Kapa.Abstractions.Capabilities;

namespace Kapa.Abstractions.States;

/// <summary>
/// <see cref="IState"/>'s property that can be read or mutated by <see cref="ICapability"/>.
/// </summary>
public interface ITrait
{
    string Name { get; }
    string Description { get; }
    public IReadOnlyCollection<IParameter> Parameters { get; }
}
