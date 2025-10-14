namespace Kapa.Fixtures.States;

// This should failed since missing StateConstructorAttribute on a decorator.
public class ManyConstructorsWithoutStateConstructorAttributeState
{
    public ManyConstructorsWithoutStateConstructorAttributeState(
        [Parameter(nameof(number))] int number
    )
    {
        Number = number;
    }

    public ManyConstructorsWithoutStateConstructorAttributeState(
        [Parameter(nameof(number))] int number,
        [Parameter(nameof(objects))] object objects
    )
    {
        Number = number;
        _ = objects;
    }

    public int Number { get; }
}
