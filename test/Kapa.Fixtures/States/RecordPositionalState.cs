namespace Kapa.Fixtures.States;

[Prototype(nameof(RecordPositionalState))]
public sealed record RecordPositionalState(
    [property: Trait("Number")] int Number,
    [property: Trait("Boolean")] bool Boolean
) { }
