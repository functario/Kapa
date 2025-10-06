using Kapa.Abstractions.Prototypes;
using Kapa.Core.Prototypes;
using Kapa.Core.Prototypes.Graphs;

namespace Kapa.Core.UnitTests.Tests.Workbench;

public class DependencyTests
{
    [Fact]
    public void MyTestMethod()
    {
        // Arrange - Create nodes from relations
        var _ = new PrototypeA();
        var relations1 = new Relations1();
        var relations2 = new Relations2();
        var relations3 = new Relations3();

        var node1 = new Node<PrototypeA>(
            typeof(Relations1),
            relations1.Mutations,
            relations1.Requirements
        );

        var node2 = new Node<PrototypeA>(
            typeof(Relations2),
            relations2.Mutations,
            relations2.Requirements
        );

        var node3 = new Node<PrototypeA>(
            typeof(Relations3),
            relations3.Mutations,
            relations3.Requirements
        );

        // Create graph
        var graph = new Graph<PrototypeA>([node1, node2, node3]);

        // Act - Resolve routes
        var routes = graph.Resolve([node3]);

        // Assert - Should find valid dependency routes
        Assert.NotEmpty(routes);

        // Example: Relations3 requires IsTraitTrue and TraitAsString
        // These can be provided by Relations1 (IsTraitTrue) and Relations2 (TraitAsString implied by mutation)
    }
}

internal sealed class PrototypeA : IPrototype
{
    public string Name => nameof(PrototypeA);

    public string Description => "Description of nameof(PrototypeA)";

    public IReadOnlyCollection<ITrait> Traits =>
        [
            new Trait(nameof(IsTraitTrue), $"description {nameof(IsTraitTrue)}", []),
            new Trait(nameof(TraitAsString), $"description {nameof(TraitAsString)}", []),
            new Trait(nameof(TraitAsInt), $"description {nameof(TraitAsInt)}", []),
            new Trait(nameof(TraitAsDouble), $"description {nameof(TraitAsDouble)}", []),
            new Trait(nameof(TraitAsLong), $"description {nameof(TraitAsLong)}", []),
        ];

    public bool IsTraitTrue { get; set; }
    public string? TraitAsString { get; set; }

    public int TraitAsInt { get; set; }
    public double TraitAsDouble { get; set; }
    public long TraitAsLong { get; set; }
}

internal sealed class Relations1 : IPrototypeRelations<PrototypeA>
{
    public ICollection<IMutation<PrototypeA>> Mutations =>
        [new Mutation<PrototypeA>(p => p.IsTraitTrue == true)];

    public ICollection<IRequirement<PrototypeA>> Requirements => [];
}

internal sealed class Relations2 : IPrototypeRelations<PrototypeA>
{
    public ICollection<IMutation<PrototypeA>> Mutations =>
        [new Mutation<PrototypeA>(p => p.TraitAsInt > 2)];

    public ICollection<IRequirement<PrototypeA>> Requirements => [];
}

internal sealed class Relations3 : IPrototypeRelations<PrototypeA>
{
    public ICollection<IMutation<PrototypeA>> Mutations => [];

    public ICollection<IRequirement<PrototypeA>> Requirements =>
        [new Requirement<PrototypeA>(p => p.IsTraitTrue == true && p.TraitAsInt > 2)];
}
