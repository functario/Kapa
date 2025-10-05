using Kapa.Abstractions.Validations;
using Kapa.Core.Validations;

namespace Kapa.Fixtures.Capabilities.WithParameters;

[CapabilityType]
public sealed class CapabilityWithAllParemeterTypes
{
    [Capability(nameof(Handle))]
    public IOutcome Handle(
        [Parameter("description str")] string str,
        [Parameter("description decimalNumber")] decimal decimalNumber,
        [Parameter("description decimalNumber")] int integerNumber,
        [Parameter("description obj")] object obj,
        [Parameter("description intCollection")] ICollection<int> intCollection,
        [Parameter("description boolean")] bool boolean
    )
    {
        return TypedOutcomes.Ok(nameof(CapabilityWithAllParemeterTypes));
    }
}
