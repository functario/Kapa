using Kapa.Abstractions.Actors;
using Kapa.Abstractions.Capabilities;

namespace Kapa.Core.Actors;

/// <inheritdoc/>
public sealed record State : IState
{
    public State(string name, string description, params IParameter[] parameters)
    {
        Name = name;
        Description = description;
        Parameters = parameters;
    }

    public string Name { get; init; }

    public string Description { get; init; }

    public IReadOnlyCollection<IParameter> Parameters { get; init; }
}
