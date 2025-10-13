namespace HomeAutomation.Demo.Tests.MermaidGraphTests;

public class DisplayNodeRequirementOptionsTests
{
    [Theory(
        DisplayName = $"Display {nameof(Graph)} depending {nameof(MermaidGraphOptions.DisplayNodeRequirementOptions)}"
    )]
    [InlineData(DisplayNodeRequirementOptions.None)]
    [InlineData(DisplayNodeRequirementOptions.ReferenceDirectEdges)]
    [InlineData(DisplayNodeRequirementOptions.ReferenceInheritedEdges)]
    public async Task Test1(DisplayNodeRequirementOptions displayNodeRequirementOption)
    {
        // Arrange
        var fullGraph = GraphCatalog.Full();
        var mermaidOptions = new MermaidGraphOptions()
        {
            DisplayNodeRequirementOptions = displayNodeRequirementOption,
        };

        // Act
        var sut = fullGraph
            .Reduce([NodeCatalog.SetThermostatSetpoint], [])
            .ToMermaidGraph(mermaidOptions);

        // Assert
        await sut.VerifyMermaidAsync();
    }
}
