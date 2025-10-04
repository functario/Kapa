using Kapa.Abstractions.States;
using Kapa.Core.States;
using Kapa.Fixtures.States;

namespace Kapa.Core.UnitTests.Tests.Extensions;

public class StateTypeExtensionsTests
{
    [Fact(
        DisplayName = $"Create {nameof(IState)} with one {nameof(ITrait)} "
            + $"from {nameof(Type)} "
            + $"decorated with {nameof(StateAttribute)}"
    )]
    public void Test1()
    {
        // Arrange
        var type = typeof(OneTraitState);

        // Act
        var sut = type.ToState();

        // Assert
        using var scope = new AssertionScope();
        sut.Should().BeAssignableTo<IState>();
        sut.Traits.Should().HaveCount(1);
        sut.Traits.First().Name.Should().Be(nameof(OneTraitState.BoolTrait));
    }

    [Fact(
        DisplayName = $"Create {nameof(IState)} with many {nameof(ITrait)} "
            + $"from {nameof(Type)} "
            + $"decorated with {nameof(StateAttribute)}"
    )]
    public void Test2()
    {
        // Arrange
        var type = typeof(ManyTraitsState);

        // Act
        var sut = type.ToState();

        // Assert
        using var scope = new AssertionScope();
        sut.Should().BeAssignableTo<IState>();
        sut.Traits.Should().HaveCount(4);
        sut.Traits.Select(x => x.Name)
            .Should()
            .BeEquivalentTo(
                [
                    nameof(ManyTraitsState.BoolTrait),
                    nameof(ManyTraitsState.RecordTrait),
                    nameof(ManyTraitsState.ClassPrimaryConstructorTrait),
                    nameof(ManyTraitsState.ClassMultiConstructorsTrait),
                ]
            );
    }

    [Fact(
        DisplayName = $"Create {nameof(IState)} without {nameof(ITrait)} "
            + $"from {nameof(Type)} "
            + $"decorated with {nameof(StateAttribute)}"
    )]
    public void Test3()
    {
        // Arrange
        var type = typeof(NoTraitState);

        // Act
        var sut = type.ToState();

        // Assert
        using var scope = new AssertionScope();
        sut.Should().BeAssignableTo<IState>();
        sut.Traits.Should().HaveCount(0);
    }

    [Fact(
        DisplayName = $"Cannot create {nameof(IState)} "
            + $"with {nameof(ITrait)} having multiple constructors "
            + $"but not decorated with {nameof(TraitConstructorAttribute)}"
    )]
    public void Test4()
    {
        // Arrange
        var type = typeof(TraitWithManyConstructorWithoutAttributeState);

        // Act
        Action sut = () => type.ToState();

        // Assert
        using var scope = new AssertionScope();
        sut.Should().ThrowExactly<MultipleTraitConstructorsException>();
    }

    [Fact(
        DisplayName = $"Cannot create {nameof(IState)} "
            + $"with {nameof(ITrait)} having multiple constructors "
            + $"decorated with {nameof(TraitConstructorAttribute)}"
    )]
    public void Test5()
    {
        // Arrange
        var type = typeof(TraitWithManyConstructorWithAttributeState);

        // Act
        Action sut = () => type.ToState();

        // Assert
        using var scope = new AssertionScope();
        sut.Should().ThrowExactly<MultipleTraitConstructorsException>();
    }

    [Fact(
        DisplayName = $"Cannot create {nameof(IState)} "
            + $"not decorated with {nameof(TraitConstructorAttribute)}"
    )]
    public void Test6()
    {
        // Arrange
        var type = typeof(object);

        // Act
        Action sut = () => type.ToState();

        // Assert
        using var scope = new AssertionScope();
        sut.Should().ThrowExactly<TypeIsNotStateException>();
    }
}
