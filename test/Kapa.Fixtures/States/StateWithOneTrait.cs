using Kapa.Core.States;

namespace Kapa.Fixtures.States;

[State(nameof(StateWithOneTrait))]
public sealed class StateWithOneTrait
{
    [Trait(nameof(Trait1))]
    public bool Trait1 { get; set; }
}
