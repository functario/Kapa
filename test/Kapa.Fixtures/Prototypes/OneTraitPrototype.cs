namespace Kapa.Fixtures.Prototypes;

[Prototype(nameof(OneTraitPrototype))]
public sealed class OneTraitPrototype
{
    [State(nameof(BoolTrait))]
    public bool BoolTrait { get; set; }
}
