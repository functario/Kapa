namespace Kapa.Fixtures.Traits;

// This should failed since missing TraitConstructorAttribute on a decorator.
public class ManyConstructorsWithoutTraitConstructorAttributeTrait
{
    public ManyConstructorsWithoutTraitConstructorAttributeTrait(
        [Parameter(nameof(number))] int number
    )
    {
        Number = number;
    }

    public ManyConstructorsWithoutTraitConstructorAttributeTrait(
        [Parameter(nameof(number))] int number,
        [Parameter(nameof(objects))] object objects
    )
    {
        Number = number;
        _ = objects;
    }

    public int Number { get; }
}
