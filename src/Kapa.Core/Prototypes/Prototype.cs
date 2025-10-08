using Kapa.Abstractions.Prototypes;

namespace Kapa.Core.Prototypes;

/// <inheritdoc/>
public sealed class Prototype : IPrototype
{
    public Prototype(string name, string description, params IState[] states)
    {
        Name = name;
        Description = description;
        States = states;
    }

    public IReadOnlyCollection<IState> States { get; }
    public string Name { get; }
    public string Description { get; }
}
