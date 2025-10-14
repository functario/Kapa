using Kapa.Core.Capabilities;

namespace HomeAutomation.Demo.Tests;

public class GraphTests
{
    private readonly MermaidGraphOptions _mermaidGraphOptions;

    public GraphTests()
    {
        _mermaidGraphOptions = new MermaidGraphOptions()
        {
            DisplayNodeRequirementOptions = DisplayNodeRequirementOptions.ReferenceInheritedEdges,
            EffectFormatOptions = EffectFormatOptions.UseId,
        };
    }

    [Fact(DisplayName = $"Display the full {nameof(Graph)}")]
    public async Task Test1()
    {
        // Arrange
        var fullGraph = GraphCatalog.Full();

        // Act
        var sut = fullGraph.ToMermaidGraph(_mermaidGraphOptions);

        // Assert
        await sut.VerifyMermaidAsync();
    }

    [Fact(DisplayName = $"Display the reduce {nameof(Graph)} for one {nameof(Capability)}")]
    public async Task Test2()
    {
        // Arrange
        var fullGraph = GraphCatalog.Full();
        INode[] includedNodes = [NodeCatalog.SetThermostatSetpoint];
        INode[] excludedNodes = [];

        // Act
        var sut = fullGraph
            .Reduce(includedNodes, excludedNodes)
            .ToMermaidGraph(_mermaidGraphOptions);

        // Assert
        await sut.VerifyMermaidAsync();
    }

    [Fact(DisplayName = $"Display the reduce {nameof(Graph)} for many {nameof(Capability)}")]
    public async Task Test3()
    {
        // Arrange
        var fullGraph = GraphCatalog.Full();
        INode[] includedNodes = [NodeCatalog.SetThermostatSetpoint, NodeCatalog.SwitchLight];
        INode[] excludedNodes = [];

        // Act
        var sut = fullGraph
            .Reduce(includedNodes, excludedNodes)
            .ToMermaidGraph(_mermaidGraphOptions);

        // Assert
        await sut.VerifyMermaidAsync();
    }
}
