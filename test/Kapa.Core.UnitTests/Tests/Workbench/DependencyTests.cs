using Kapa.Abstractions.Prototypes;
using Kapa.Core.Prototypes;

namespace Kapa.Core.UnitTests.Tests.Workbench;

public class DependencyTests
{
    [Fact]
    public void MyTestMethod()
    {
        // Arrange
        var prototypeA = new PrototypeA();
        var relations1 = new Relations1();
        var relations2 = new Relations2();
        var relations3 = new Relations3();
        // Act

        // Assert
    }
}

internal sealed class PrototypeA : IPrototype
{
    public string Name => nameof(PrototypeA);

    public string Description => "Description of nameof(PrototypeA)";

    public IReadOnlyCollection<ITrait> Traits =>
        [
            new Trait(nameof(IsTraitTrue), $"description {nameof(IsTraitTrue)}", []),
            new Trait(nameof(TraitAsString), $"description {nameof(TraitAsString)}", []),
            new Trait(nameof(TraitAsInt), $"description {nameof(TraitAsInt)}", []),
        ];

    public bool IsTraitTrue { get; set; }
    public string? TraitAsString { get; set; }

    public int TraitAsInt { get; set; }
}

internal sealed class Relations1 : IPrototypeRelations<PrototypeA>
{
    public ICollection<IMutation<PrototypeA>> Mutations =>
        [new Mutation<PrototypeA>(p => p.IsTraitTrue, p => p.IsTraitTrue = true)];

    public ICollection<IRequirement<PrototypeA>> Requirements => [];
}

internal sealed class Relations2 : IPrototypeRelations<PrototypeA>
{
    public ICollection<IMutation<PrototypeA>> Mutations =>
        [
            new Mutation<PrototypeA>(p => p.IsTraitTrue, p => p.IsTraitTrue = false),
            new Mutation<PrototypeA>(p => p.TraitAsString, p => p.TraitAsString = "not null"),
        ];

    public ICollection<IRequirement<PrototypeA>> Requirements =>
        [new Requirement<PrototypeA>(p => p.TraitAsInt > 2)];
}

internal sealed class Relations3 : IPrototypeRelations<PrototypeA>
{
    public ICollection<IMutation<PrototypeA>> Mutations =>
        [new Mutation<PrototypeA>(p => p.TraitAsInt, p => p.TraitAsInt = 100)];

    public ICollection<IRequirement<PrototypeA>> Requirements =>
        [
            new Requirement<PrototypeA>(p => p.IsTraitTrue == true),
            new Requirement<PrototypeA>(p => p.TraitAsString != null),
        ];
}
