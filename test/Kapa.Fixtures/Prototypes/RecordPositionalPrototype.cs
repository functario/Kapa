namespace Kapa.Fixtures.Prototypes;

[Prototype(nameof(RecordPositionalPrototype))]
public sealed record RecordPositionalPrototype(
    [property: State("Number")] int Number,
    [property: State("Boolean")] bool Boolean
) { }
