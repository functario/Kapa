using Kapa.Abstractions.Prototypes;
using Kapa.Abstractions.Validations;
using Kapa.Core.Factories;
using Kapa.Core.Prototypes;
using Kapa.Core.Validations;

namespace Kapa.Core.UnitTests.Tests.Workbench;

public class DependencyTests
{
    [Fact]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1303")]
    public void MyTestMethod()
    {
        // Arrange - Create nodes from relations

        var capability1 = typeof(CapabilityType1).ToCapabilityType().Capabilities.Single();
        var capability2 = typeof(CapabilityType2).ToCapabilityType().Capabilities.Single();
        var capability3 = typeof(CapabilityType3).ToCapabilityType().Capabilities.Single();

        // Example: Capability3 requires IsTraitTrue and TraitAsInt
        // Route 1: Capability1 → Capability2 → Capability3
        // Route 2: Capability2 → Capability1 → Capability3
    }
}

[CapabilityType]
internal sealed class CapabilityType1
{
    [Capability(nameof(Capability1))]
    [Relations<Relations1>()]
    public IOutcome Capability1()
    {
        return TypedOutcomes.Ok(nameof(CapabilityType1));
    }
}

[CapabilityType]
internal sealed class CapabilityType2
{
    [Capability(nameof(Capability2))]
    [Relations<Relations2>()]
    public IOutcome Capability2()
    {
        return TypedOutcomes.Ok(nameof(CapabilityType2));
    }
}

[CapabilityType]
internal sealed class CapabilityType3
{
    [Capability(nameof(Capabilit3))]
    [Relations<Relations3>()]
    public IOutcome Capabilit3()
    {
        return TypedOutcomes.Ok(nameof(CapabilityType3));
    }
}

[Prototype(nameof(PrototypeA))]
internal sealed class PrototypeA : IHasTrait
{
    public bool IsTraitTrue { get; set; }
    public string? TraitAsString { get; set; }

    public int TraitAsInt { get; set; }
    public double TraitAsDouble { get; set; }
    public long TraitAsLong { get; set; }
}

internal sealed class Relations1 : IPrototypeRelations<IHasTrait>
{
    public ICollection<IMutation<IHasTrait>> Mutations =>
        [MutationFactory.Create<PrototypeA>(p => p.IsTraitTrue == true)];

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
