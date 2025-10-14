using Kapa.Abstractions.Capabilities;

namespace Kapa.Abstractions.Actors;

/// <summary>
/// <see cref="IActor"/>'s property that can be read or mutated by <see cref="ICapability"/>.
/// </summary>
public interface IState
{
    string Name { get; }
    string Description { get; }
    public IReadOnlyCollection<IParameter> Parameters { get; }
}
