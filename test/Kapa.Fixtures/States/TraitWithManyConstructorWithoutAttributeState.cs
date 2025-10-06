using Kapa.Fixtures.Traits;

namespace Kapa.Fixtures.States;

[Prototype(nameof(RecordPositionalState))]
public class TraitWithManyConstructorWithoutAttributeState
{
    [Trait(nameof(ManyConstructorsWithoutTraitConstructorAttributeTrait))]
    public ManyConstructorsWithoutTraitConstructorAttributeTrait? Trait1 { get; set; }
}
