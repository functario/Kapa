using Kapa.Abstractions.Actors;

namespace Kapa.Core.Actors;

/// <inheritdoc/>
public class Actor : IActor
{
    public Actor(string name, string description, params IState[] states)
    {
        Name = name;
        Description = description;
        States = states;
    }

    public IReadOnlyCollection<IState> States { get; }
    public string Name { get; }
    public string Description { get; }

    public Task DispatchEvents() => throw new NotImplementedException();
}
