using Kapa.Abstractions.Prototypes;

namespace Kapa.Core.Prototypes;

public class DependencyAnalyzer<TPrototype>
    where TPrototype : IPrototype, new()
{
    // Find simple resolutions within a single descriptor
    public IEnumerable<(
        IMutation<TPrototype> Mutation,
        IRequirement<TPrototype> Requirement
    )> FindSimpleResolutions(IPrototypeRelations<TPrototype> relations)
    {
        ArgumentNullException.ThrowIfNull(relations);
        throw new NotImplementedException();
    }

    // Find composed resolutions across multiple descriptors
    public IEnumerable<ComposedResolution<TPrototype>> FindComposedResolutions(
        params (Type ClassType, IPrototypeRelations<TPrototype> Descriptor)[] relations
    )
    {
        ArgumentNullException.ThrowIfNull(relations);
        throw new NotImplementedException();
    }
}

public sealed record ComposedResolution<TPrototype>(
    Type DependentClass,
    IRequirement<TPrototype> Requirement,
    ICollection<(Type ClassType, IMutation<TPrototype> Mutation)> ResolvingMutations
)
    where TPrototype : IPrototype, new();
