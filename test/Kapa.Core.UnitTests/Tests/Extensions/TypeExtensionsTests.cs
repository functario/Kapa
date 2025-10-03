using Kapa.Fixtures.Capabilities.Inheritances;
using Kapa.Fixtures.Capabilities.Inheritances.Statics;

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
            + $"decorated with {nameof(CapabilityTypeAttribute)} "
            + $"is {nameof(ICapabilityType)}"
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
            + $"not decorated with {nameof(CapabilityTypeAttribute)} "
            + $"is not {nameof(ICapabilityType)}"
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
        sut.Should().ThrowExactly<DuplicateCapabilityDescriptionsException>();
    }

    [Fact(
        DisplayName = $"Create {nameof(ICapabilityType)} from static {nameof(Type)} "
            + $"decorated with {nameof(CapabilityTypeAttribute)}"
    )]
    public void Test9()
    {
        // Arrange
        var type = typeof(StaticCapabilityType);

        // Act
        var sut = type.ToCapabilityType();

        // Assert
        sut.Should().BeAssignableTo<ICapabilityType>();
    }

    [Fact(
        DisplayName = $"Create child {nameof(ICapabilityType)} from {nameof(Type)} "
            + $"decorated with {nameof(CapabilityTypeAttribute)} and "
            + $"inheriting from many parent levels"
    )]
    public void Test10()
    {
        // Arrange
        var type = typeof(ChildLevel3CapabilityType);

        // Act
        var sut = type.ToCapabilityType();

        // Assert
        using var scope = new AssertionScope();
        sut.Should().BeAssignableTo<ICapabilityType>();
        sut.Capabilities.Select(x => x.Name)
            .Should()
            .BeEquivalentTo(
                [
                    nameof(ParentCapabitlity.Handle),
                    nameof(ChildLevel1CapabilityType.Handle1),
                    nameof(ChildLevel2CapabilityType.Handle2),
                    nameof(ChildLevel3CapabilityType.Handle3),
                ],
                options => options.WithoutStrictOrdering()
            );
    }

    [Fact(
        DisplayName = $"Create child {nameof(ICapabilityType)} from {nameof(Type)} "
            + $"not decorated with {nameof(CapabilityTypeAttribute)} but "
            + $"inheriting from decorated {nameof(ICapabilityType)}"
    )]
    public void Test11()
    {
        // Arrange
        var type = typeof(ChildWithoutExplicitCapabilityTypeAttribute);

        // Act
        var sut = type.ToCapabilityType();

        // Assert
        using var scope = new AssertionScope();
        sut.Should().BeAssignableTo<ICapabilityType>();
        sut.Capabilities.Select(x => x.Name)
            .Should()
            .BeEquivalentTo(
                [
                    nameof(ParentCapabitlity.Handle),
                    nameof(ChildLevel1CapabilityType.Handle1),
                    nameof(ChildLevel2CapabilityType.Handle2),
                    nameof(ChildLevel3CapabilityType.Handle3),
                ],
                options => options.WithoutStrictOrdering()
            );
    }

    [Fact(
        DisplayName = $"Create great child {nameof(ICapabilityType)} from {nameof(Type)} "
            + $"not decorated with {nameof(CapabilityTypeAttribute)} but "
            + $"inheriting from decorated {nameof(ICapabilityType)}"
    )]
    public void Test12()
    {
        // Arrange
        var type = typeof(GreatChildWithoutExplicitCapabilityTypeAttribute);

        // Act
        var sut = type.ToCapabilityType();

        // Assert
        using var scope = new AssertionScope();
        sut.Should().BeAssignableTo<ICapabilityType>();
        sut.Capabilities.Select(x => x.Name)
            .Should()
            .BeEquivalentTo(
                [
                    nameof(ParentCapabitlity.Handle),
                    nameof(ChildLevel1CapabilityType.Handle1),
                    nameof(ChildLevel2CapabilityType.Handle2),
                    nameof(ChildLevel3CapabilityType.Handle3),
                ],
                options => options.WithoutStrictOrdering()
            );
    }

    [Fact(
        DisplayName = $"Create static child {nameof(ICapabilityType)} from {nameof(Type)} "
            + $"decorated with {nameof(CapabilityTypeAttribute)} and "
            + $"inheriting from many parent levels"
    )]
    public void Test13()
    {
        // Arrange
        var type = typeof(StaticChildLevel3CapabilityType);

        // Act
        var sut = type.ToCapabilityType();

        // Assert
        using var scope = new AssertionScope();
        sut.Should().BeAssignableTo<ICapabilityType>();
        sut.Capabilities.Select(x => x.Name)
            .Should()
            .BeEquivalentTo(
                [
                    nameof(StaticParentCapabitlity.Handle),
                    nameof(StaticChildLevel1CapabilityType.Handle1),
                    nameof(StaticChildLevel2CapabilityType.Handle2),
                    nameof(StaticChildLevel3CapabilityType.Handle3),
                ],
                options => options.WithoutStrictOrdering()
            );
    }

    [Fact(
        DisplayName = $"Create static child {nameof(ICapabilityType)} from {nameof(Type)} "
            + $"not decorated with {nameof(CapabilityTypeAttribute)} but "
            + $"inheriting from decorated {nameof(ICapabilityType)}"
    )]
    public void Test14()
    {
        // Arrange
        var type = typeof(StaticChildWithoutExplicitCapabilityTypeAttribute);

        // Act
        var sut = type.ToCapabilityType();

        // Assert
        using var scope = new AssertionScope();
        sut.Should().BeAssignableTo<ICapabilityType>();
        sut.Capabilities.Select(x => x.Name)
            .Should()
            .BeEquivalentTo(
                [
                    nameof(StaticParentCapabitlity.Handle),
                    nameof(StaticChildLevel1CapabilityType.Handle1),
                    nameof(StaticChildLevel2CapabilityType.Handle2),
                    nameof(StaticChildLevel3CapabilityType.Handle3),
                ],
                options => options.WithoutStrictOrdering()
            );
    }

    [Fact(
        DisplayName = $"Create static great child {nameof(ICapabilityType)} from {nameof(Type)} "
            + $"not decorated with {nameof(CapabilityTypeAttribute)} but "
            + $"inheriting from decorated {nameof(ICapabilityType)}"
    )]
    public void Test15()
    {
        // Arrange
        var type = typeof(StaticGreatChildWithoutExplicitCapabilityTypeAttribute);

        // Act
        var sut = type.ToCapabilityType();

        // Assert
        using var scope = new AssertionScope();
        sut.Should().BeAssignableTo<ICapabilityType>();
        sut.Capabilities.Select(x => x.Name)
            .Should()
            .BeEquivalentTo(
                [
                    nameof(StaticParentCapabitlity.Handle),
                    nameof(StaticChildLevel1CapabilityType.Handle1),
                    nameof(StaticChildLevel2CapabilityType.Handle2),
                    nameof(StaticChildLevel3CapabilityType.Handle3),
                ],
                options => options.WithoutStrictOrdering()
            );
    }
}
