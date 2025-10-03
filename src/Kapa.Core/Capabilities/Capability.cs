using Kapa.Abstractions.Capabilities;

namespace Kapa.Core.Capabilities;

public record Capability : ICapability
{
    public Capability(string name, string description)
    {
        Name = name;
        Description = description;
    }

    public Capability(string name, string description, IParameter[] parameters)
    {
        Name = name;
        Description = description;
        Parameters = parameters;
    }

    public string Name { get; }

    public string Description { get; }

    public IReadOnlyCollection<IParameter> Parameters { get; init; } = [];
}
