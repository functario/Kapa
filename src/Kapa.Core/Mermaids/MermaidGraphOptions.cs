using Kapa.Abstractions.Actors;
using Kapa.Abstractions.Graphs;
using Kapa.Core.Capabilities;

namespace Kapa.Core.Mermaids;

public record MermaidGraphOptions
{
    public bool ReplaceNodeNameBySource { get; set; }
    public bool DisplayNodeDescription { get; set; } = true;
    public bool DisplayOnlyNodeMissingRequirements { get; set; }
    public MermaidGraphOrientations GraphOrientations { get; set; }
    public DisplayNodeRequirementOptions DisplayNodeRequirementOptions { get; set; }
    public EffectDisplayOptions EffectDisplayOptions { get; set; }
}

[Flags]
public enum EffectDisplayOptions
{
    None = 0,

    /// <summary>
    /// Use the <see cref="IEffect{IGeneratedActor}"/> <see cref="IEffect{IGeneratedActor}.Id"/>.
    /// </summary>
    UseId = 1,

    /// <summary>
    /// Use the <see cref="IEffect{IGeneratedActor}"/> <see cref="IEffect{IGeneratedActor}.Description"/>.
    /// </summary>
    UseDescription = 2,
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
    /// Including resolutions inherited
    /// from previous <see cref="INode"/>s <see cref="Relations.Requirements"/>.
    /// </summary>
    ReferenceInheritedEdges,
}
