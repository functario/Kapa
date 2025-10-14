namespace HomeAutomation.Demo.Tests.MermaidGraphTests;

public class DisplayNodeRequirementOptionsTests
{
    // TODO: Need a matrix since EffectFormatOptions and DisplayNodeRequirementOptions impact each others
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
            EffectFormatOptions = EffectFormatOptions.UseDescription,
        };

        // Act
        var sut = fullGraph
            .Reduce([NodeCatalog.SetThermostatSetpoint], [])
            .ToMermaidGraph(mermaidOptions);

        // Assert
        await sut.VerifyMermaidAsync();
    }

    [Theory(
        DisplayName = $"Display {nameof(Graph)} with missing {nameof(Graph.MissingRequirements)}"
    )]
    [InlineData(DisplayNodeRequirementOptions.None)]
    [InlineData(DisplayNodeRequirementOptions.ReferenceDirectEdges)]
    [InlineData(DisplayNodeRequirementOptions.ReferenceInheritedEdges)]
    public async Task Test2(DisplayNodeRequirementOptions displayNodeRequirementOption)
    {
        // Arrange
        var fullGraph = GraphCatalog.Full();
        var mermaidOptions = new MermaidGraphOptions()
        {
            DisplayNodeRequirementOptions = displayNodeRequirementOption,
            EffectFormatOptions = EffectFormatOptions.UseDescription,
        };

        // Act
        var sut = fullGraph
            .Reduce(
                [NodeCatalog.SetThermostatSetpoint],
                [NodeCatalog.Setup, NodeCatalog.AuthenticateAsync]
            )
            .ToMermaidGraph(mermaidOptions);

        // Assert
        await sut.VerifyMermaidAsync();
    }
}
