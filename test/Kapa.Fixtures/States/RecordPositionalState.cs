using Kapa.Core.States;

namespace Kapa.Fixtures.States;

public sealed record RecordPositionalState(
    [property: Trait("Number")] int Number,
    [property: Trait("Boolean")] bool Boolean
)
{ }
