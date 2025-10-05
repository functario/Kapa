using Kapa.Abstractions.Validations;
using Kapa.Core.Validations;
using Kapa.Fixtures.Capabilities.WithTypedOutcomes;

namespace Kapa.Core.UnitTests.Tests.Workbench;

public class OutcomeTests
{
    [Fact]
    public void Test1()
    {
        // Arrange
        var capa = new OkOutcomeCapacity();

        // Act
        var sut = capa.Handle();

        // Assert
        sut.Should().BeAssignableTo<IOutcome>();
        sut.Status.Should().Be(OutcomeStatus.Ok);
    }

    [Fact]
    public void Test2()
    {
        // Arrange
        var capa = new OkOutcomeCapacity();

        // Act
        var sut = capa.Handle();

        // Assert
        sut.Should().BeAssignableTo<IOutcome>();
        sut.Status.Should().Be(OutcomeStatus.Ok);
    }

    [Theory]
    [InlineData(OutcomeStatus.Ok, typeof(Ok<string>))]
    [InlineData(OutcomeStatus.Fail, typeof(Fail<string>))]
    [InlineData(OutcomeStatus.RulesFail, typeof(RulesFail<string>))]
    public void Test3(OutcomeStatus outcomeStatus, Type innerType)
    {
        // Arrange
        var capa = new OkOrFailOrRulesFailOutcomeCapacity(outcomeStatus);

        // Act
        var sut = capa.Handle();

        // Assert
        sut.Should().BeAssignableTo<IOutcome>();
        sut.Should().BeOfType<Outcomes<Ok<string>, Fail<string>, RulesFail<string>>>();
        sut.Outcome.Should().BeAssignableTo(innerType);
    }

    [Fact]
    public void Test4()
    {
        // Arrange
        var capa = new OkOfTCapacity();

        // Act
        var sut = capa.Handle();

        // Assert
        sut.Should().BeAssignableTo<IOutcome>();
        sut.Status.Should().Be(OutcomeStatus.Ok);
    }

    [Fact(DisplayName = $"Infer {nameof(IOutcomeMetadata)} from {nameof(ICapability)} creation.")]
    public void Test5()
    {
        // Arrange
        var type = typeof(OkOrFailOrRulesFailOutcomeCapacity);

        // Act
        var sut = type.GetCapabilities();

        // Assert
    }
}
