using AwesomeAssertions.Execution;

namespace Kapa.Core.UnitTests.Tests.Extensions;

public class TypeExtensionsTests
{
    [Fact(
        DisplayName = $"Create {nameof(ICapabilityType)} from {nameof(Type)} "
            + $"decorated with {nameof(CapabilityTypeAttribute)}"
    )]
    public void Test1()
    {
        // Arrange
        var sut = typeof(SimpleCapabilityType);

        // Act
        var result = sut.ToCapabilityType();

        // Assert
        using var scope = new AssertionScope();
        result.Should().NotBeNull();
        result.Should().BeAssignableTo<ICapabilityType>();
        result.Capabilities.Should().NotBeEmpty();
    }

    [Fact(
        DisplayName = $"Cannot create {nameof(ICapabilityType)} from {nameof(Type)} "
            + $"not decorated with {nameof(CapabilityTypeAttribute)}"
    )]
    public void Test2()
    {
        // Arrange

        // Act

        // Assert
    }

    [Fact(
        DisplayName = $"Get {nameof(ICapability)}s from {nameof(Type)} "
            + $"decorated with {nameof(CapabilityTypeAttribute)}"
    )]
    public void Test3()
    {
        // Arrange

        // Act

        // Assert
    }

    [Fact(
        DisplayName = $"Confirm that {nameof(Type)} "
            + $"decorated with {nameof(CapabilityTypeAttribute)}"
            + $" is {nameof(ICapabilityType)}"
    )]
    public void Test4()
    {
        // Arrange

        // Act

        // Assert
    }

    [Fact(
        DisplayName = $"Confirm that {nameof(Type)} "
            + $"not decorated with {nameof(CapabilityTypeAttribute)}"
            + $" is not {nameof(ICapabilityType)}"
    )]
    public void Test5()
    {
        // Arrange

        // Act

        // Assert
    }

    [Fact(
        DisplayName = $"Cannot create {nameof(ICapabilityType)} "
            + $"without ${nameof(ICapability)}"
    )]
    public void Test6()
    {
        // Arrange

        // Act

        // Assert
    }
}
