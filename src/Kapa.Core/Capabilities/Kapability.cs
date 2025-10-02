using Kapa.Abstractions.Capabilities;

namespace Kapa.Core.Capabilities;

public class Kapability : IKapability
{
    public Kapability(ICollection<IKapaStep> steps)
    {
        Steps = [.. steps];
    }

    public IReadOnlyCollection<IKapaStep> Steps { get; }
}
