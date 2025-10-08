namespace Kapa.Fixtures.States;

public sealed class ClassMultiConstructorsState
{
    // This constructor should be ignored since it does not have StateConstructor.
    // Which is required if multiple constructors with ParameterAttribute.
    public ClassMultiConstructorsState(
        [Parameter(nameof(number))] int number,
        [Parameter(nameof(boolean))] bool boolean
    )
    {
        Number = number;
        Boolean = boolean;
    }

    [StateConstructor]
    public ClassMultiConstructorsState(
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
