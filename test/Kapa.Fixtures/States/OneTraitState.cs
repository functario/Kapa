using Kapa.Core.States;

namespace Kapa.Fixtures.States;

[State(nameof(OneTraitState))]
public sealed class OneTraitState
{
    [Trait(nameof(BoolTrait))]
    public bool BoolTrait { get; set; }
}
