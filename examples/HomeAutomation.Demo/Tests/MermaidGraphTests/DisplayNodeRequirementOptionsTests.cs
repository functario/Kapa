namespace HomeAutomation.Demo.Tests.MermaidGraphTests;

public class DisplayNodeRequirementOptionsTests
{
    [Theory(
        DisplayName = $"Display {nameof(Graph)} depending {nameof(MermaidGraphOptions.DisplayNodeRequirementOptions)}"
    )]
    [InlineData(DisplayNodeRequirementOptions.None)]
    [InlineData(DisplayNodeRequirementOptions.ReferenceDirectEdges)]
    [InlineData(DisplayNodeRequirementOptions.ReferenceRecursiveEdges)]
    public async Task Test1(DisplayNodeRequirementOptions displayNodeRequirementOption)
    {
        // Arrange
        var fullGraph = GraphCatalog.Full();
        INode[] includedNodes = [NodeCatalog.SetThermostatSetpoint, NodeCatalog.SwitchLight];
        INode[] excludedNodes = [];
        var mermaidOptions = new MermaidGraphOptions()
        {
            DisplayNodeRequirementOptions = displayNodeRequirementOption,
        };

        // Act
        var sut = fullGraph.Reduce(includedNodes, excludedNodes).ToMermaidGraph(mermaidOptions);

        // Assert
        await sut.VerifyMermaidAsync();
    }
}
