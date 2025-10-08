using Kapa.Abstractions.Prototypes;
using Kapa.Fixtures.States;

namespace Kapa.Fixtures.Prototypes;

[Prototype(nameof(ManyStatesPrototype))]
public sealed class ManyStatesPrototype : IGeneratedPrototype
{
    [State(nameof(BoolState))]
    public bool BoolState { get; set; }

    [State(nameof(RecordState))]
    public RecordPositionalConstructorState? RecordState { get; set; }

    [State(nameof(ClassPrimaryConstructorState))]
    public ClassPrimaryConstructorState? ClassPrimaryConstructorState { get; set; }

    [State(nameof(ClassMultiConstructorsState))]
    public ClassMultiConstructorsState? ClassMultiConstructorsState { get; set; }
}
