using Kapa.Abstractions.Prototypes;

namespace Kapa.Core.Capabilities;

public class PrototypeRelations : IPrototypeRelations<IHasTrait>
{
    public PrototypeRelations()
    {
        Mutations = [];
        Requirements = [];
    }

    public PrototypeRelations(
        ICollection<IMutation<IHasTrait>> mutations,
        ICollection<IRequirement<IHasTrait>> requirements
    )
    {
        Mutations = mutations;
        Requirements = requirements;
    }

    public ICollection<IMutation<IHasTrait>> Mutations { get; }
    public ICollection<IRequirement<IHasTrait>> Requirements { get; }
}
