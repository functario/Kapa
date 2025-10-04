using Kapa.Core.States;
using Kapa.Fixtures.Traits;

namespace Kapa.Fixtures.States;

[State(nameof(RecordPositionalState))]
public class TraitWithManyConstructorWithAttributeState
{
    [Trait(nameof(ManyConstructorsWithTraitConstructorAttributeTrait))]
    public ManyConstructorsWithTraitConstructorAttributeTrait? Trait1 { get; set; }
}
