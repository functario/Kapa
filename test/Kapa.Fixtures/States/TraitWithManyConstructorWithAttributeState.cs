using Kapa.Fixtures.Traits;

namespace Kapa.Fixtures.States;

[Prototype(nameof(RecordPositionalState))]
public class TraitWithManyConstructorWithAttributeState
{
    [Trait(nameof(ManyConstructorsWithTraitConstructorAttributeTrait))]
    public ManyConstructorsWithTraitConstructorAttributeTrait? Trait1 { get; set; }
}
