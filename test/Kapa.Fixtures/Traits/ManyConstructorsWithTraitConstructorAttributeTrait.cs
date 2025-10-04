namespace Kapa.Fixtures.Traits;

// This should failed since more than one TraitConstructorAttribute.
public class ManyConstructorsWithTraitConstructorAttributeTrait
{
    public ManyConstructorsWithTraitConstructorAttributeTrait(
        [Parameter(nameof(number))] int number
    )
    {
        Number = number;
    }

    public ManyConstructorsWithTraitConstructorAttributeTrait(
        [Parameter(nameof(number))] int number,
        [Parameter(nameof(objects))] object objects
    )
    {
        Number = number;
        _ = objects;
    }

    public int Number { get; }
}
