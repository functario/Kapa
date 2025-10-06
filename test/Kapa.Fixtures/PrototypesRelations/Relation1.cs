using Kapa.Abstractions.Prototypes;
using Kapa.Fixtures.Prototypes;

namespace Kapa.Fixtures.PrototypesRelations;

public sealed class Relation1 : IPrototypeRelations<IHasTrait>
{
    public ICollection<IMutation<IHasTrait>> Mutations =>
        [new Mutation<IHasTrait>(p => ((ManyTraitsPrototype)p).BoolTrait == true)];

    public ICollection<IRequirement<IHasTrait>> Requirements => [];
}
