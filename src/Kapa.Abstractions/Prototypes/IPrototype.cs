using Kapa.Abstractions.Capabilities;

namespace Kapa.Abstractions.Prototypes;

/// <summary>
/// A <see cref="IPrototype"/> with <see cref="IState"/>
/// that can be read or mutated by <see cref="ICapability"/>.
/// </summary>
public interface IPrototype : IGeneratedPrototype
{
    string Name { get; }
    public string Description { get; }
    public IReadOnlyCollection<IState> States { get; }
}
