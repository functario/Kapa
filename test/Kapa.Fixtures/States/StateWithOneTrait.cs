using Kapa.Core.States;

namespace Kapa.Fixtures.States;

[State(nameof(StateWithOneTrait))]
public sealed class StateWithOneTrait
{
    [Trait(nameof(Trait1))]
    public bool Trait1 { get; set; }

    [Trait(nameof(Trait2))]
    public RecordTrait? Trait2 { get; set; }

    [Trait(nameof(Trait3))]
    public ClassTrait? Trait3 { get; set; }
}

public sealed record RecordTrait(
    [Parameter(nameof(Number))] int Number,
    [Parameter(nameof(Boolean))] bool Boolean
)
{ }

public sealed class ClassTrait(
    [Parameter(nameof(number))] int number,
    [Parameter(nameof(boolean))] bool boolean
)
{
    public int Number => number;
    public bool Boolean => boolean;
}

public sealed class ClassMultiConstructorsTrait
{
    public ClassMultiConstructorsTrait(
        [Parameter(nameof(number))] int number,
        [Parameter(nameof(boolean))] bool boolean
    )
    {
        Number = number;
        Boolean = boolean;
    }

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
