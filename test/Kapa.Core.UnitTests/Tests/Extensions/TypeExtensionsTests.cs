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
        var type = typeof(OneCapability);

        // Act
        var sut = type.ToCapabilityType();

        // Assert
        using var scope = new AssertionScope();
        sut.Should().BeAssignableTo<ICapabilityType>();
    }

    [Fact(
        DisplayName = $"Cannot create {nameof(ICapabilityType)} from {nameof(Type)} "
            + $"not decorated with {nameof(CapabilityTypeAttribute)}"
    )]
    public void Test2()
    {
        // Arrange
        var type = typeof(NotCapabilityType);

        // Act
        Action sut = () => type.ToCapabilityType();

        // Assert
        using var scope = new AssertionScope();
        sut.Should().ThrowExactly<TypeIsNotCapabilityException>();
    }

    [Fact(
        DisplayName = $"Get one {nameof(ICapability)}s from {nameof(Type)} "
            + $"decorated with {nameof(CapabilityTypeAttribute)}"
    )]
    public void Test3()
    {
        // Arrange
        var type = typeof(OneCapability);

        // Act
        var sut = type.GetCapabilities();

        // Assert
        using var scope = new AssertionScope();
        sut.Should().ContainSingle();
        sut.First().Name.Should().Be(nameof(OneCapability.Handle));
    }

    [Fact(
        DisplayName = $"Get many {nameof(ICapability)}s from {nameof(Type)} "
            + $"decorated with {nameof(CapabilityTypeAttribute)}"
    )]
    public void Test4()
    {
        // Arrange
        var type = typeof(ManyCapabilities);

        // Act
        var sut = type.GetCapabilities();

        // Assert
        using var scope = new AssertionScope();
        sut.Should().HaveCount(3);
        sut.Select(x => x.Name)
            .Should()
            .BeEquivalentTo(
                [
                    nameof(ManyCapabilities.Handle1),
                    nameof(ManyCapabilities.Handle2),
                    nameof(ManyCapabilities.Handle3),
                ],
                options => options.WithStrictOrdering()
            );
    }

    [Fact(
        DisplayName = $"Confirm that {nameof(Type)} "
            + $"decorated with {nameof(CapabilityTypeAttribute)}"
            + $" is {nameof(ICapabilityType)}"
    )]
    public void Test5()
    {
        // Arrange
        var type = typeof(ManyCapabilities);

        // Act
        var sut = type.IsCapabilityType();

        // Assert
        sut.Should().BeTrue();
    }

    [Fact(
        DisplayName = $"Confirm that {nameof(Type)} "
            + $"not decorated with {nameof(CapabilityTypeAttribute)}"
            + $" is not {nameof(ICapabilityType)}"
    )]
    public void Test6()
    {
        // Arrange
        var type = typeof(NotCapabilityType);

        // Act
        var sut = type.IsCapabilityType();

        // Assert
        sut.Should().BeFalse();
    }

    [Fact(
        DisplayName = $"Cannot create {nameof(ICapabilityType)} "
            + $"without ${nameof(ICapability)}"
    )]
    public void Test7()
    {
        // Arrange
        var type = typeof(EmptyCapabilityType);

        // Act
        Action sut = () => type.ToCapabilityType();

        // Assert
        using var scope = new AssertionScope();
        sut.Should().ThrowExactly<MissingCapabilityException>();
    }

    [Fact(
        DisplayName = $"Cannot create {nameof(ICapabilityType)} "
            + $"with ${nameof(ICapability)} duplicated descriptions"
    )]
    public void Test8()
    {
        // Arrange
        var type = typeof(DuplicatedCapabilityDescriptions);

        // Act
        Action sut = () => type.ToCapabilityType();

        // Assert
        using var scope = new AssertionScope();
        sut.Should().ThrowExactly<DuplicateCapabilityDescriptionsException>();
    }
}
