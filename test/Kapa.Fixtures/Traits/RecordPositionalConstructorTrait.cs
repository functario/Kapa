namespace Kapa.Fixtures.Traits;

public sealed record RecordPositionalConstructorTrait(
    [Parameter(nameof(Number))] int Number,
    [Parameter(nameof(Boolean))] bool Boolean
) { }
