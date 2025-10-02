namespace Kapa.Abstractions.Capabilities;

public interface IKapability
{
    public IReadOnlyCollection<IKapaStep> KapaSteps { get; }
}
