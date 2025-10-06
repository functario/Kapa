using Kapa.Fixtures.Traits;

namespace Kapa.Fixtures.Prototypes;

[Prototype(nameof(ManyTraitsPrototype))]
public sealed class ManyTraitsPrototype
{
    [Trait(nameof(BoolTrait))]
    public bool BoolTrait { get; set; }

    [Trait(nameof(RecordTrait))]
    public RecordPositionalConstructorTrait? RecordTrait { get; set; }

    [Trait(nameof(ClassPrimaryConstructorTrait))]
    public ClassPrimaryConstructorTrait? ClassPrimaryConstructorTrait { get; set; }

    [Trait(nameof(ClassMultiConstructorsTrait))]
    public ClassMultiConstructorsTrait? ClassMultiConstructorsTrait { get; set; }
}
