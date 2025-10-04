using Kapa.Fixtures.Capabilities.WithTypedOutcomes;
using Newtonsoft.Json;

namespace Kapa.Core.UnitTests.Tests.Workbench;

public class OutcomeTests
{
    [Fact]
    public void Test1()
    {
        // Arrange
        var capa = new SringOutcomeCapacity();

        // Act
        var sut = capa.Handle();

        // Assert
        sut.Should().BeAssignableTo<IOutcome>();
        sut.Status.Should().Be(OutcomeStatus.Ok);
        sut.Kind.Should().Be(Kinds.StringKind);
        sut.Value.Should().BeOfType<string>();
    }

    [Fact]
    public void Test2()
    {
        // Arrange
        var capa = new NoneOutcomeCapacity();

        // Act
        var sut = capa.Handle();

        // Assert
        sut.Should().BeAssignableTo<IOutcome>();
        sut.Status.Should().Be(OutcomeStatus.Ok);
        sut.Kind.Should().Be(Kinds.NoneKind);
        sut.Value.Should().BeNull();
    }

    [Fact]
    public void Test3()
    {
        // Arrange
        var capa = new SringOutcomeCapacity();
        var outcome = capa.Handle();

        // Act
        var sut = JsonConvert.SerializeObject(outcome, Formatting.Indented);

        // Assert
    }
}
