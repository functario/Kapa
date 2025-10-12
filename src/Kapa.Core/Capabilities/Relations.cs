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
        ICollection<IEffect<IGeneratedActor>> mutations,
        ICollection<IEffect<IGeneratedActor>> requirements
    )
    {
        Mutations = mutations;
        Requirements = requirements;
    }

    public ICollection<IEffect<IGeneratedActor>> Mutations { get; }
    public ICollection<IEffect<IGeneratedActor>> Requirements { get; }
}
