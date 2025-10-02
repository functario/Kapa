namespace Kapa.Abstractions.Capabilities;

public interface IKapability
{
    public IReadOnlyCollection<IKapaStep> Steps { get; }
}
