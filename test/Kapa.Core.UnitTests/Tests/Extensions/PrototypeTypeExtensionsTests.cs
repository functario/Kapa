using Kapa.Abstractions.Prototypes;
using Kapa.Core.Prototypes;
using Kapa.Fixtures.Prototypes;

namespace Kapa.Core.UnitTests.Tests.Extensions;

public class PrototypeTypeExtensionsTests
{
    [Fact(
        DisplayName = $"Create {nameof(IPrototype)} with one {nameof(IState)} "
            + $"from {nameof(Type)} "
            + $"decorated with {nameof(PrototypeAttribute)}"
    )]
    public void Test1()
    {
        // Arrange
        var type = typeof(OneStatePrototype);

        // Act
        var sut = type.ToPrototype();

        // Assert
        using var scope = new AssertionScope();
        sut.Should().BeAssignableTo<IPrototype>();
        sut.States.Should().HaveCount(1);
        sut.States.First().Name.Should().Be(nameof(OneStatePrototype.BoolState));
    }

    [Fact(
        DisplayName = $"Create {nameof(IPrototype)} with many {nameof(IState)} "
            + $"from {nameof(Type)} "
            + $"decorated with {nameof(PrototypeAttribute)}"
    )]
    public void Test2()
    {
        // Arrange
        var type = typeof(ManyStatesPrototype);

        // Act
        var sut = type.ToPrototype();

        // Assert
        using var scope = new AssertionScope();
        sut.Should().BeAssignableTo<IPrototype>();
        sut.States.Should().HaveCount(4);
        sut.States.Select(x => x.Name)
            .Should()
            .BeEquivalentTo(
                [
                    nameof(ManyStatesPrototype.BoolState),
                    nameof(ManyStatesPrototype.RecordState),
                    nameof(ManyStatesPrototype.ClassPrimaryConstructorState),
                    nameof(ManyStatesPrototype.ClassMultiConstructorsState),
                ]
            );
    }

    [Fact(
        DisplayName = $"Create {nameof(IPrototype)} without {nameof(IState)} "
            + $"from {nameof(Type)} "
            + $"decorated with {nameof(PrototypeAttribute)}"
    )]
    public void Test3()
    {
        // Arrange
        var type = typeof(NoStatePrototype);

        // Act
        var sut = type.ToPrototype();

        // Assert
        using var scope = new AssertionScope();
        sut.Should().BeAssignableTo<IPrototype>();
        sut.States.Should().HaveCount(0);
    }

    [Fact(
        DisplayName = $"Cannot create {nameof(IPrototype)} "
            + $"with {nameof(IState)} having multiple constructors "
            + $"but not decorated with {nameof(StateConstructorAttribute)}"
    )]
    public void Test4()
    {
        // Arrange
        var type = typeof(StateWithManyConstructorWithoutAttributePrototype);

        // Act
        Action sut = () => type.ToPrototype();

        // Assert
        using var scope = new AssertionScope();
        sut.Should().ThrowExactly<MultipleStateConstructorsException>();
    }

    [Fact(
        DisplayName = $"Cannot create {nameof(IPrototype)} "
            + $"with {nameof(IState)} having multiple constructors "
            + $"decorated with {nameof(StateConstructorAttribute)}"
    )]
    public void Test5()
    {
        // Arrange
        var type = typeof(StateWithManyConstructorWithAttributePrototype);

        // Act
        Action sut = () => type.ToPrototype();

        // Assert
        using var scope = new AssertionScope();
        sut.Should().ThrowExactly<MultipleStateConstructorsException>();
    }

    [Fact(
        DisplayName = $"Cannot create {nameof(IPrototype)} "
            + $"not decorated with {nameof(StateConstructorAttribute)}"
    )]
    public void Test6()
    {
        // Arrange
        var type = typeof(object);

        // Act
        Action sut = () => type.ToPrototype();

        // Assert
        using var scope = new AssertionScope();
        sut.Should().ThrowExactly<TypeIsNotPrototypeException>();
    }
}
