using Kapa.Abstractions.Capabilities;

namespace Kapa.Core.Capabilities;

public record KapaStep : IKapaStep
{
    public KapaStep(string name, string description)
    {
        Name = name;
        Description = description;
        Title = null;
    }

    public KapaStep(string name, string description, string? title)
    {
        Name = name;
        Description = description;
        Title = title;
    }

    public string Name { get; }

    public string Description { get; }

    public string? Title { get; }

    public IReadOnlyCollection<IKapaParam> Parameters { get; init; } = [];
}
