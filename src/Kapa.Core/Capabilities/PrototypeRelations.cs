using Kapa.Abstractions.Prototypes;

namespace Kapa.Core.Capabilities;

public class PrototypeRelations : IPrototypeRelations<IGeneratedPrototype>
{
    public PrototypeRelations()
    {
        Mutations = [];
        Requirements = [];
    }

    public PrototypeRelations(
        ICollection<IMutation<IGeneratedPrototype>> mutations,
        ICollection<IRequirement<IGeneratedPrototype>> requirements
    )
    {
        Mutations = mutations;
        Requirements = requirements;
    }

    public ICollection<IMutation<IGeneratedPrototype>> Mutations { get; }
    public ICollection<IRequirement<IGeneratedPrototype>> Requirements { get; }
}
