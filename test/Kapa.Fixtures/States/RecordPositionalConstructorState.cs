namespace Kapa.Fixtures.States;

public sealed record RecordPositionalConstructorState(
    [Parameter(nameof(Number))] int Number,
    [Parameter(nameof(Boolean))] bool Boolean
)
{ }
