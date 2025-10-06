using Kapa.Abstractions.Prototypes;
using Kapa.Core.Prototypes;

namespace Kapa.Core.UnitTests.Tests.Workbench;

public class DependencyTests
{
    [Fact]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1303")]
    public void MyTestMethod()
    {
        // Arrange - Create nodes from relations
        var _ = new PrototypeA();
        var relations1 = new Relations1();
        var relations2 = new Relations2();
        var relations3 = new Relations3();

        // Example: Relations3 requires IsTraitTrue and TraitAsInt
        // Route 1: Relations1 → Relations2 → Relations3
        // Route 2: Relations2 → Relations1 → Relations3
    }
}

internal sealed class PrototypeA : IHasTrait
{
    public IReadOnlyCollection<ITrait> Traits =>
        [
            new Trait(nameof(IsTraitTrue), $"description {nameof(IsTraitTrue)}", []),
            new Trait(nameof(TraitAsString), $"description {nameof(TraitAsString)}", []),
            new Trait(nameof(TraitAsInt), $"description {nameof(TraitAsInt)}", []),
            new Trait(nameof(TraitAsDouble), $"description {nameof(TraitAsDouble)}", []),
            new Trait(nameof(TraitAsLong), $"description {nameof(TraitAsLong)}", []),
        ];

    public bool IsTraitTrue { get; set; }
    public string? TraitAsString { get; set; }

    public int TraitAsInt { get; set; }
    public double TraitAsDouble { get; set; }
    public long TraitAsLong { get; set; }
}

internal sealed class Relations1 : IPrototypeRelations<IHasTrait>
{
    public ICollection<IMutation<IHasTrait>> Mutations =>
        [new Mutation<IHasTrait>(p => ((PrototypeA)p).IsTraitTrue == true)];

    public ICollection<IRequirement<IHasTrait>> Requirements => [];
}

internal sealed class Relations2 : IPrototypeRelations<IHasTrait>
{
    public ICollection<IMutation<IHasTrait>> Mutations =>
        [new Mutation<IHasTrait>(p => ((PrototypeA)p).TraitAsInt > 2)];

    public ICollection<IRequirement<IHasTrait>> Requirements => [];
}

internal sealed class Relations3 : IPrototypeRelations<IHasTrait>
{
    public ICollection<IMutation<IHasTrait>> Mutations => [];

    public ICollection<IRequirement<IHasTrait>> Requirements =>
        [
            new Requirement<IHasTrait>(p =>
                ((PrototypeA)p).IsTraitTrue == true && ((PrototypeA)p).TraitAsInt > 2
            ),
        ];
}
