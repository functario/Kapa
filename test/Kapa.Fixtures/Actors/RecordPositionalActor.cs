namespace Kapa.Fixtures.Actors;

[Actor(nameof(RecordPositionalActor))]
public sealed record RecordPositionalActor(
    [property: State("Number")] int Number,
    [property: State("Boolean")] bool Boolean
)
{ }
