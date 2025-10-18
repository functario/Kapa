using Kapa.Abstractions.Actors;
using Kapa.Abstractions.Capabilities;
using Kapa.Abstractions.Validations;

namespace Kapa.Abstractions.BDD;

public interface IKapaStep
{
    public ICapability Capability { get; }
    public Func<CancellationToken, Task<IOutcome>> ExecuteAsync { get; }

    public IActor Actor { get; }
}
