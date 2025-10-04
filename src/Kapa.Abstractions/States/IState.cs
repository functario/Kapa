using Kapa.Abstractions.Capabilities;

namespace Kapa.Abstractions.States;

/// <summary>
/// A state with properties that can be read or mutated by <see cref="ICapability"/>.
/// </summary>
public interface IState
{
    string Name { get; }
    public string Description { get; }
    public IReadOnlyCollection<ITrait> Traits { get; }
}
