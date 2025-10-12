namespace Kapa.Abstractions.Actors;

public interface IEffect<out TGeneratedActor>
    where TGeneratedActor : IGeneratedActor
{
    public string Id { get; }
    public string Description { get; }
    public Func<IGeneratedActor, bool> Predicate { get; }

    public bool AreEqual(IEffect<IGeneratedActor> other);
}
