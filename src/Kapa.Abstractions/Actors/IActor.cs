using Kapa.Abstractions.Capabilities;

namespace Kapa.Abstractions.Actors;

/// <summary>
/// A <see cref="IActor"/> with <see cref="IState"/>
/// that can be read or mutated by <see cref="ICapability"/>.
/// </summary>
public interface IActor : IGeneratedActor
{
    string Name { get; }
    public string Description { get; }
    public IReadOnlyCollection<IState> States { get; }
}
