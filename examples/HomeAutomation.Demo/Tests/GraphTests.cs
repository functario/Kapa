using Kapa.Core.Capabilities;

namespace HomeAutomation.Demo.Tests;

public class GraphTests
{
    [Fact(DisplayName = $"Display the full {nameof(Graph)}")]
    public async Task Test1()
    {
        // Arrange
        var fullGraph = GraphCatalog.Full();

        // Act
        var sut = fullGraph.ToMermaidGraph();

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
        var sut = fullGraph.Reduce(includedNodes, excludedNodes).ToMermaidGraph();

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
        var sut = fullGraph.Reduce(includedNodes, excludedNodes);

        // Assert
        await sut.ToMermaidGraph().VerifyMermaidAsync();
    }
}
