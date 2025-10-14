namespace Kapa.Fixtures.States;

// This should failed since more than one StateConstructorAttribute.
public class ManyConstructorsWithStateConstructorAttributeState
{
    public ManyConstructorsWithStateConstructorAttributeState(
        [Parameter(nameof(number))] int number
    )
    {
        Number = number;
    }

    public ManyConstructorsWithStateConstructorAttributeState(
        [Parameter(nameof(number))] int number,
        [Parameter(nameof(objects))] object objects
    )
    {
        Number = number;
        _ = objects;
    }

    public int Number { get; }
}
