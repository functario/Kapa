namespace Kapa.Abstractions.Prototypes;

public interface IPrototypeRelations<TPrototype>
    where TPrototype : IPrototype, new()
{
    public ICollection<IMutation<TPrototype>> Mutations { get; }
    public ICollection<IRequirement<TPrototype>> Requirements { get; }
}
