using Kapa.Fixtures.Traits;

namespace Kapa.Fixtures.Prototypes;

[Prototype(nameof(RecordPositionalPrototype))]
public class TraitWithManyConstructorWithoutAttributePrototype
{
    [State(nameof(ManyConstructorsWithoutTraitConstructorAttributeTrait))]
    public ManyConstructorsWithoutTraitConstructorAttributeTrait? Trait1 { get; set; }
}
