using Kapa.Abstractions.Actors;
using Kapa.Abstractions.BDD;
using Kapa.Abstractions.Capabilities;
using Kapa.Abstractions.Validations;

namespace Kapa.Core.BDD;

public sealed record KapaStep(
    IActor Actor,
    ICapability Capability,
    Func<CancellationToken, Task<IOutcome>> ExecuteAsync
) : IKapaStep
{ }
