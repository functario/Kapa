namespace Kapa.Fixtures.Prototypes;

[Prototype(nameof(OneStatePrototype))]
public sealed class OneStatePrototype
{
    [State(nameof(BoolState))]
    public bool BoolState { get; set; }
}
