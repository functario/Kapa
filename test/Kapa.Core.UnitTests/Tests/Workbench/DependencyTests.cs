using Kapa.Abstractions.Graphs;
using Kapa.Abstractions.Prototypes;
using Kapa.Abstractions.Validations;
using Kapa.Core.Factories;
using Kapa.Core.Graphs;
using Kapa.Core.Prototypes;
using Kapa.Core.Validations;

namespace Kapa.Core.UnitTests.Tests.Workbench;

public class DependencyTests
{
    [Fact]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1303")]
    public void ResolveCapabilitiesDependenciesWithGraphTheory()
    {
        // Arrange - Create nodes from relations

        var capabilityNode1 = typeof(CapabilityType1)
            .ToCapabilityType()
            .Capabilities.Single()
            .ToNode();

        var capabilityNode2 = typeof(CapabilityType2)
            .ToCapabilityType()
            .Capabilities.Single()
            .ToNode();

        // CapabilityType3 has 3 methods with CapabilityAttribute
        var capabilitiesNode3 = typeof(CapabilityType3)
            .ToCapabilityType()
            .Capabilities.Select(x => x.ToNode())
            .ToArray();

        var capabilityNode3 = capabilitiesNode3[0];
        var capabilityNode4 = capabilitiesNode3[1];
        var capabilityNode5 = capabilitiesNode3[2];

        INode[] nodes =
        [
            capabilityNode1,
            capabilityNode2,
            capabilityNode3,
            capabilityNode4,
            capabilityNode5,
        ];

        var graph = new Graph(nodes);

        var focusedGraph = graph.Reduce([capabilityNode4, capabilityNode5, capabilityNode3], []);

        var a = focusedGraph.ToMermaidGraph();

        // Should include all three nodes: Capability3 (waypoint) + Capability1 & Capability2 (dependencies)
        focusedGraph.Nodes.Count.Should().Be(5);
        focusedGraph.Nodes.Should().Contain(capabilityNode1);
        focusedGraph.Nodes.Should().Contain(capabilityNode2);
        focusedGraph.Nodes.Should().Contain(capabilityNode3);
    }
}

[CapabilityType]
internal sealed class CapabilityType1
{
    [Capability(nameof(Capability1))]
    [Relations<Relations1>()]
    public IOutcome Capability1()
    {
        return TypedOutcomes.Ok(nameof(CapabilityType1));
    }
}

[CapabilityType]
internal sealed class CapabilityType2
{
    [Capability(nameof(Capability2))]
    [Relations<Relations2>()]
    public IOutcome Capability2()
    {
        return TypedOutcomes.Ok(nameof(CapabilityType2));
    }
}

[CapabilityType]
internal sealed class CapabilityType3
{
    [Capability(nameof(Capability3))]
    [Relations<Relations3>()]
    public IOutcome Capability3()
    {
        return TypedOutcomes.Ok(nameof(CapabilityType3));
    }

    // No relations
    [Capability(nameof(Capability4))]
    public IOutcome Capability4()
    {
        return TypedOutcomes.Ok(nameof(CapabilityType3));
    }

    [Capability(nameof(Capability5))]
    [Relations<Relations4>()]
    public IOutcome Capability5()
    {
        return TypedOutcomes.Ok(nameof(CapabilityType3));
    }
}

[Prototype(nameof(PrototypeA))]
internal sealed class PrototypeA : IGeneratedPrototype
{
    public bool IsTraitTrue { get; set; }
    public string? TraitAsString { get; set; }

    public int TraitAsInt { get; set; }
    public double TraitAsDouble { get; set; }
    public long TraitAsLong { get; set; }
}

internal sealed class Relations1 : IPrototypeRelations<IGeneratedPrototype>
{
    public ICollection<IMutation<IGeneratedPrototype>> Mutations =>
        [MutationFactory.Create<PrototypeA>(p => p.IsTraitTrue == true)];

    public ICollection<IRequirement<IGeneratedPrototype>> Requirements => [];
}

internal sealed class Relations2 : IPrototypeRelations<IGeneratedPrototype>
{
    public ICollection<IMutation<IGeneratedPrototype>> Mutations =>
        [MutationFactory.Create<PrototypeA>(p => p.TraitAsInt > 2)];

    public ICollection<IRequirement<IGeneratedPrototype>> Requirements => [];
}

internal sealed class Relations3 : IPrototypeRelations<IGeneratedPrototype>
{
    public ICollection<IMutation<IGeneratedPrototype>> Mutations => [];

    public ICollection<IRequirement<IGeneratedPrototype>> Requirements =>
        [
            RequirementFactory.Create<PrototypeA>(p => p.TraitAsInt > 2),
            RequirementFactory.Create<PrototypeA>(p => p.IsTraitTrue == true),
        ];
}

internal sealed class Relations4 : IPrototypeRelations<IGeneratedPrototype>
{
    public ICollection<IMutation<IGeneratedPrototype>> Mutations =>
        [MutationFactory.Create<PrototypeA>(p => p.TraitAsDouble > 2)];

    public ICollection<IRequirement<IGeneratedPrototype>> Requirements => [];
}
