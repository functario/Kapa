namespace Kapa.Fixtures.Prototypes;

[Prototype(nameof(RecordPositionalPrototype))]
public sealed record RecordPositionalPrototype(
    [property: Trait("Number")] int Number,
    [property: Trait("Boolean")] bool Boolean
)
{ }
