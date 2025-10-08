using Kapa.Fixtures.Traits;

namespace Kapa.Fixtures.Prototypes;

[Prototype(nameof(RecordPositionalPrototype))]
public class TraitWithManyConstructorWithAttributePrototype
{
    [State(nameof(ManyConstructorsWithTraitConstructorAttributeTrait))]
    public ManyConstructorsWithTraitConstructorAttributeTrait? Trait1 { get; set; }
}
