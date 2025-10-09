using Kapa.Abstractions.Actors;

namespace Kapa.Core.Capabilities;

public class Relations : IRelations<IGeneratedActor>
{
    public Relations()
    {
        Mutations = [];
        Requirements = [];
    }

    public Relations(
        ICollection<IMutation<IGeneratedActor>> mutations,
        ICollection<IRequirement<IGeneratedActor>> requirements
    )
    {
        Mutations = mutations;
        Requirements = requirements;
    }

    public ICollection<IMutation<IGeneratedActor>> Mutations { get; }
    public ICollection<IRequirement<IGeneratedActor>> Requirements { get; }
}
