using Kapa.Fixtures.Traits;

namespace Kapa.Fixtures.Prototypes;

[Prototype(nameof(RecordPositionalPrototype))]
public class TraitWithManyConstructorWithAttributePrototype
{
    [Trait(nameof(ManyConstructorsWithTraitConstructorAttributeTrait))]
    public ManyConstructorsWithTraitConstructorAttributeTrait? Trait1 { get; set; }
}
