using Kapa.Abstractions.Graphs;
using Kapa.Core.Capabilities;

namespace Kapa.Core.Mermaids;

public record MermaidGraphOptions
{
    public bool ReplaceNameBySource { get; set; }
    public bool DisplayDescription { get; set; } = true;
    public bool DisplayRequirementsOnEdges { get; set; } = true;
    public bool DisplayOnlyNodeMissingRequirements { get; set; }
    public bool DisplayEdgeReference { get; set; } = true;
    public MermaidGraphOrientations GraphOrientations { get; set; }
    public DisplayNodeRequirementOptions DisplayNodeRequirementOptions { get; set; }
}

public enum DisplayNodeRequirementOptions
{
    /// <summary>
    /// Do not display the <see cref="IEdge"/> resolving the <see cref="Relations.Requirements"/>
    /// </summary>
    None,

    /// <summary>
    /// Display only the direct <see cref="IEdge"/>s resolving the <see cref="Relations.Requirements"/>.
    /// Some <see cref="Relations.Requirements"/> could be already
    /// resolved via a previous <see cref="INode"/>s <see cref="Relations.Requirements"/>.
    /// </summary>
    ReferenceDirectEdges,

    /// <summary>
    /// Display all <see cref="IEdge"/>s resolving the <see cref="Relations.Requirements"/>.
    /// Including <see cref="Relations.Requirements"/> the ones
    /// resolved via a previous <see cref="INode"/>s <see cref="Relations.Requirements"/>.
    /// </summary>
    ReferenceRecursiveEdges,
}
