using Kapa.Abstractions.Capabilities;

namespace Kapa.Core.Capabilities;

public class Kapability : IKapability
{
    public Kapability(ICollection<IKapaStep> kapaSteps)
    {
        KapaSteps = [.. kapaSteps];
    }

    public IReadOnlyCollection<IKapaStep> KapaSteps { get; }
}
