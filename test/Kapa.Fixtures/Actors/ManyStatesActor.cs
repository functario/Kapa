using Kapa.Abstractions.Actors;
using Kapa.Fixtures.States;

namespace Kapa.Fixtures.Actors;

[Actor(nameof(ManyStatesActor))]
public sealed class ManyStatesActor : IGeneratedActor
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
