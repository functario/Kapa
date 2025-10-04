using Kapa.Abstractions.Capabilities;
using Kapa.Abstractions.States;

namespace Kapa.Core.States;

/// <inheritdoc/>
public sealed record Trait : ITrait
{
    public Trait(string name, string description, params IParameter[] parameters)
    {
        Name = name;
        Description = description;
        Parameters = parameters;
    }

    public string Name { get; init; }

    public string Description { get; init; }

    public IReadOnlyCollection<IParameter> Parameters { get; init; }
}
