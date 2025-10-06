using Kapa.Abstractions.Prototypes;

namespace Kapa.Core.Prototypes;

/// <inheritdoc/>
public sealed class Prototype : IPrototype
{
    public Prototype(string name, string description, params ITrait[] traits)
    {
        Name = name;
        Description = description;
        Traits = traits;
    }

    public IReadOnlyCollection<ITrait> Traits { get; }
    public string Name { get; }
    public string Description { get; }
}
