using Kapa.Core.States;
using Kapa.Fixtures.Traits;

namespace Kapa.Fixtures.States;

[State(nameof(ManyTraitsState))]
public sealed class ManyTraitsState
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
