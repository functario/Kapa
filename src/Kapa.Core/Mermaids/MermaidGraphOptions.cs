namespace Kapa.Core.Mermaids;

public record MermaidGraphOptions
{
    public bool ReplaceNameBySource { get; set; }
    public bool DisplayDescription { get; set; } = true;
    public bool DisplayRequirementsOnEdges { get; set; } = true;
    public bool DisplayNodeRequirements { get; set; } = true;
    public bool DisplayOnlyNodeMissingRequirements { get; set; }
    public bool DisplayEdgeReference { get; set; } = true;
    public MermaidGraphOrientations GraphOrientations { get; set; }
}
