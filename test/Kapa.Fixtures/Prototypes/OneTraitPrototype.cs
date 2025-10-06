namespace Kapa.Fixtures.Prototypes;

[Prototype(nameof(OneTraitPrototype))]
public sealed class OneTraitPrototype
{
    [Trait(nameof(BoolTrait))]
    public bool BoolTrait { get; set; }
}
