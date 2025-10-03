using Kapa.Abstractions.Capabilities;

namespace Kapa.Core.Capabilities;

public record Capability : ICapability
{
    public Capability(string name, string description)
    {
        Name = name;
        Description = description;
        Title = null;
    }

    public Capability(string name, string description, string? title)
    {
        Name = name;
        Description = description;
        Title = title;
    }

    public string Name { get; }

    public string Description { get; }

    public string? Title { get; }

    public IReadOnlyCollection<IParameter> Parameters { get; init; } = [];
}
