namespace Kapa.Fixtures.States.Traits;

public sealed class ClassPrimaryConstructorTrait(
    [Parameter(nameof(number))] int number,
    [Parameter(nameof(boolean))] bool boolean
)
{
    public int Number => number;
    public bool Boolean => boolean;
}
