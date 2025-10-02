namespace Kapa.Abstractions.Capabilities;

public interface IKapaStep
{
    public string Name { get; }
    public string Description { get; }
    public string? Title { get; }
}
