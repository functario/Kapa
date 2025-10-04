using Kapa.Core.States;
using Kapa.Fixtures.Traits;

namespace Kapa.Fixtures.States;

[State(nameof(RecordPositionalState))]
public class TraitWithManyConstructorWithoutAttributeState
{
    [Trait(nameof(ManyConstructorsWithoutTraitConstructorAttributeTrait))]
    public ManyConstructorsWithoutTraitConstructorAttributeTrait? Trait1 { get; set; }
}
