using Kapa.Fixtures.States;

namespace Kapa.Fixtures.Prototypes;

[Prototype(nameof(RecordPositionalPrototype))]
public class StateWithManyConstructorWithoutAttributePrototype
{
    [State(nameof(ManyConstructorsWithoutStateConstructorAttributeState))]
    public ManyConstructorsWithoutStateConstructorAttributeState? State1 { get; set; }
}
