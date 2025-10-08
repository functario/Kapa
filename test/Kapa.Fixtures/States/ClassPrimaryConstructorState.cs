namespace Kapa.Fixtures.States;

public sealed class ClassPrimaryConstructorState(
    [Parameter(nameof(number))] int number,
    [Parameter(nameof(boolean))] bool boolean
)
{
    public int Number => number;
    public bool Boolean => boolean;
}
