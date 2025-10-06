namespace Kapa.Fixtures.States;

[Prototype(nameof(OneTraitState))]
public sealed class OneTraitState
{
    [Trait(nameof(BoolTrait))]
    public bool BoolTrait { get; set; }
}
