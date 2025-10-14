using Kapa.Fixtures.States;

namespace Kapa.Fixtures.Actors;

[Actor(nameof(RecordPositionalActor))]
public class StateWithManyConstructorWithAttributeActor
{
    [State(nameof(ManyConstructorsWithStateConstructorAttributeState))]
    public ManyConstructorsWithStateConstructorAttributeState? State1 { get; set; }
}
