namespace Kapa.Fixtures.Traits;

public sealed class ClassMultiConstructorsTrait
{
    // This constructor should be ignored since it does not have TraitConstructor.
    // Which is required if multiple constructors with ParameterAttribute.
    public ClassMultiConstructorsTrait(
        [Parameter(nameof(number))] int number,
        [Parameter(nameof(boolean))] bool boolean
    )
    {
        Number = number;
        Boolean = boolean;
    }

    [TraitConstructor]
    public ClassMultiConstructorsTrait(
        [Parameter(nameof(number))] int number,
        [Parameter(nameof(boolean))] bool boolean,
        [Parameter(nameof(nullObject))] object? nullObject
    )
    {
        Number = number;
        Boolean = boolean;
        NullObject = nullObject;
    }

    public int Number { get; }
    public bool Boolean { get; }
    public object? NullObject { get; }
}
