namespace Kapa.Abstractions.Prototypes;

public interface IPrototypeRelations<TGeneratedPrototype>
    where TGeneratedPrototype : IGeneratedPrototype
{
    public ICollection<IMutation<TGeneratedPrototype>> Mutations { get; }
    public ICollection<IRequirement<TGeneratedPrototype>> Requirements { get; }
}
