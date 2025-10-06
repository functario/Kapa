namespace Kapa.Abstractions.Prototypes;

public interface IPrototypeRelations<THasTrait>
    where THasTrait : IGeneratedPrototype
{
    public ICollection<IMutation<THasTrait>> Mutations { get; }
    public ICollection<IRequirement<THasTrait>> Requirements { get; }
}
