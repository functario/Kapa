using Kapa.Abstractions.States;

namespace Kapa.Core.States;

/// <inheritdoc/>
public sealed class State : IState
{
    public State(string name, string description, params ITrait[] traits)
    {
        Name = name;
        Description = description;
        Traits = traits;
    }

    public IReadOnlyCollection<ITrait> Traits { get; }
    public string Name { get; }
    public string Description { get; }
}
