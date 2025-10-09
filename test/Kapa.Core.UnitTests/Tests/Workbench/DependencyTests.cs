using System.Reflection;
using Kapa.Abstractions.Actors;
using Kapa.Abstractions.Graphs;
using Kapa.Abstractions.Validations;
using Kapa.Core.Actors;
using Kapa.Core.Factories;
using Kapa.Core.Graphs;
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

        /*
         
         Pour le graph, ajouter Grid position dependant du niveau de dependance et mutation
Plus de dependence une Capabilities a plus elle sera dans le coin bas/gauche
Les Capabilities sans dependance et sans mutation seront sur la premiere ligne, en sans allant vers la droite
         
         
         
         
         */

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
        return TypedOutcomes.Ok(MethodBase.GetCurrentMethod());
    }
}

[CapabilityType]
internal sealed class CapabilityType2
{
    [Capability(nameof(Capability2))]
    [Relations<Relations2>()]
    public IOutcome Capability2()
    {
        return TypedOutcomes.Ok(MethodBase.GetCurrentMethod());
    }
}

[CapabilityType]
internal sealed class CapabilityType3
{
    [Capability(nameof(Capability3))]
    [Relations<Relations3>()]
    public IOutcome Capability3()
    {
        return TypedOutcomes.Ok(MethodBase.GetCurrentMethod());
    }

    // No relations
    [Capability(nameof(Capability4))]
    public IOutcome Capability4()
    {
        return TypedOutcomes.Ok(MethodBase.GetCurrentMethod());
    }

    [Capability(nameof(Capability5))]
    [Relations<Relations4>()]
    public IOutcome Capability5()
    {
        return TypedOutcomes.Ok(MethodBase.GetCurrentMethod());
    }
}

[Actor(nameof(ActorA))]
internal sealed class ActorA : IGeneratedActor
{
    public bool IsStateTrue { get; set; }
    public string? StateAsString { get; set; }

    public int StateAsInt { get; set; }
    public double StateAsDouble { get; set; }
    public long StateAsLong { get; set; }
}

internal sealed class Relations1 : IRelations<IGeneratedActor>
{
    public ICollection<IMutation<IGeneratedActor>> Mutations =>
        [MutationFactory.Create<ActorA>(p => p.IsStateTrue == true)];

    public ICollection<IRequirement<IGeneratedActor>> Requirements => [];
}

internal sealed class Relations2 : IRelations<IGeneratedActor>
{
    public ICollection<IMutation<IGeneratedActor>> Mutations =>
        [MutationFactory.Create<ActorA>(p => p.StateAsInt > 2)];

    public ICollection<IRequirement<IGeneratedActor>> Requirements => [];
}

internal sealed class Relations3 : IRelations<IGeneratedActor>
{
    public ICollection<IMutation<IGeneratedActor>> Mutations => [];

    public ICollection<IRequirement<IGeneratedActor>> Requirements =>
        [
            RequirementFactory.Create<ActorA>(p => p.StateAsInt > 2),
            RequirementFactory.Create<ActorA>(p => p.IsStateTrue == true),
        ];
}

internal sealed class Relations4 : IRelations<IGeneratedActor>
{
    public ICollection<IMutation<IGeneratedActor>> Mutations =>
        [MutationFactory.Create<ActorA>(p => p.StateAsDouble > 2)];

    public ICollection<IRequirement<IGeneratedActor>> Requirements => [];
}
