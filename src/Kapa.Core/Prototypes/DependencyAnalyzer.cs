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
        var results = new List<(IMutation<TPrototype>, IRequirement<TPrototype>)>();

        foreach (var mutation in relations.Mutations)
        {
            foreach (var requirement in relations.Requirements)
            {
                if (requirement.ReferencedProperties.Contains(mutation.MutatedProperty))
                {
                    results.Add((mutation, requirement));
                }
            }
        }

        return results;
    }

    // Find composed resolutions across multiple descriptors
    public IEnumerable<ComposedResolution<TPrototype>> FindComposedResolutions(
        params (Type ClassType, IPrototypeRelations<TPrototype> Descriptor)[] relations
    )
    {
        ArgumentNullException.ThrowIfNull(relations);
        var results = new List<ComposedResolution<TPrototype>>();

        foreach (var (targetClass, targetDescriptor) in relations)
        {
            foreach (var requirement in targetDescriptor.Requirements)
            {
                var resolvingMutations =
                    new List<(Type ClassType, IMutation<TPrototype> Mutation)>();

                // Find all mutations from all classes that affect this requirement
                foreach (var (mutatorClass, mutatorDescriptor) in relations)
                {
                    foreach (var mutation in mutatorDescriptor.Mutations)
                    {
                        if (requirement.ReferencedProperties.Contains(mutation.MutatedProperty))
                        {
                            resolvingMutations.Add((mutatorClass, mutation));
                        }
                    }
                }

                if (resolvingMutations.Count != 0)
                {
                    results.Add(
                        new ComposedResolution<TPrototype>(
                            targetClass,
                            requirement,
                            resolvingMutations.ToArray()
                        )
                    );
                }
            }
        }

        return results;
    }
}

public sealed record ComposedResolution<TPrototype>(
    Type DependentClass,
    IRequirement<TPrototype> Requirement,
    ICollection<(Type ClassType, IMutation<TPrototype> Mutation)> ResolvingMutations
)
    where TPrototype : IPrototype, new();
