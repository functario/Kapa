using Kapa.Abstractions.Prototypes;

namespace Kapa.Core.Prototypes;

/// <inheritdoc/>
public sealed class Prototype : IPrototype
{
    public Prototype(string name, string description, params IState[] traits)
    {
        Name = name;
        Description = description;
        States = traits;
    }

    public IReadOnlyCollection<IState> States { get; }
    public string Name { get; }
    public string Description { get; }
}
