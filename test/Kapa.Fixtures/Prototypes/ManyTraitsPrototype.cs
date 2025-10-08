using Kapa.Abstractions.Prototypes;
using Kapa.Fixtures.Traits;

namespace Kapa.Fixtures.Prototypes;

[Prototype(nameof(ManyTraitsPrototype))]
public sealed class ManyTraitsPrototype : IGeneratedPrototype
{
    [State(nameof(BoolTrait))]
    public bool BoolTrait { get; set; }

    [State(nameof(RecordTrait))]
    public RecordPositionalConstructorTrait? RecordTrait { get; set; }

    [State(nameof(ClassPrimaryConstructorTrait))]
    public ClassPrimaryConstructorTrait? ClassPrimaryConstructorTrait { get; set; }

    [State(nameof(ClassMultiConstructorsTrait))]
    public ClassMultiConstructorsTrait? ClassMultiConstructorsTrait { get; set; }
}
