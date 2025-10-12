namespace Kapa.Abstractions.Actors;

public interface IEffect<out TGeneratedActor>
    where TGeneratedActor : IGeneratedActor
{
    public string Description { get; }
    public Func<IGeneratedActor, bool> Predicate { get; }
}
