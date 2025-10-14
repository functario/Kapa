using Kapa.Fixtures.States;

namespace Kapa.Fixtures.Actors;

[Actor(nameof(RecordPositionalActor))]
public class StateWithManyConstructorWithoutAttributeActor
{
    [State(nameof(ManyConstructorsWithoutStateConstructorAttributeState))]
    public ManyConstructorsWithoutStateConstructorAttributeState? State1 { get; set; }
}
