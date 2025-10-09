namespace Kapa.Abstractions.Actors;

public interface IRelations<TGeneratedActor>
    where TGeneratedActor : IGeneratedActor
{
    public ICollection<IMutation<TGeneratedActor>> Mutations { get; }
    public ICollection<IRequirement<TGeneratedActor>> Requirements { get; }
}
