namespace Kapa.Fixtures.Actors;

[Actor(nameof(OneStateActor))]
public sealed class OneStateActor
{
    [State(nameof(BoolState))]
    public bool BoolState { get; set; }
}
