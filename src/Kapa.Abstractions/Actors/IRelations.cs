namespace Kapa.Abstractions.Actors;

public interface IRelations<TGeneratedActor>
    where TGeneratedActor : IGeneratedActor
{
    public ICollection<IEffect<TGeneratedActor>> Mutations { get; }
    public ICollection<IEffect<TGeneratedActor>> Requirements { get; }
}
