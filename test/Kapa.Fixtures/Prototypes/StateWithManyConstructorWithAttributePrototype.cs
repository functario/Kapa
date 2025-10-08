using Kapa.Fixtures.States;

namespace Kapa.Fixtures.Prototypes;

[Prototype(nameof(RecordPositionalPrototype))]
public class StateWithManyConstructorWithAttributePrototype
{
    [State(nameof(ManyConstructorsWithStateConstructorAttributeState))]
    public ManyConstructorsWithStateConstructorAttributeState? State1 { get; set; }
}
