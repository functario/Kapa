using Kapa.Abstractions.Actors;
using Kapa.Core.Actors;
using Kapa.Fixtures.Actors;

namespace Kapa.Core.UnitTests.Tests.Extensions;

public class ActorTypeExtensionsTests
{
    [Fact(
        DisplayName = $"Create {nameof(IActor)} with one {nameof(IState)} "
            + $"from {nameof(Type)} "
            + $"decorated with {nameof(ActorAttribute)}"
    )]
    public void Test1()
    {
        // Arrange
        var type = typeof(OneStateActor);

        // Act
        var sut = type.ToActor();

        // Assert
        using var scope = new AssertionScope();
        sut.Should().BeAssignableTo<IActor>();
        sut.States.Should().HaveCount(1);
        sut.States.First().Name.Should().Be(nameof(OneStateActor.BoolState));
    }

    [Fact(
        DisplayName = $"Create {nameof(IActor)} with many {nameof(IState)} "
            + $"from {nameof(Type)} "
            + $"decorated with {nameof(ActorAttribute)}"
    )]
    public void Test2()
    {
        // Arrange
        var type = typeof(ManyStatesActor);

        // Act
        var sut = type.ToActor();

        // Assert
        using var scope = new AssertionScope();
        sut.Should().BeAssignableTo<IActor>();
        sut.States.Should().HaveCount(4);
        sut.States.Select(x => x.Name)
            .Should()
            .BeEquivalentTo(
                [
                    nameof(ManyStatesActor.BoolState),
                    nameof(ManyStatesActor.RecordState),
                    nameof(ManyStatesActor.ClassPrimaryConstructorState),
                    nameof(ManyStatesActor.ClassMultiConstructorsState),
                ]
            );
    }

    [Fact(
        DisplayName = $"Create {nameof(IActor)} without {nameof(IState)} "
            + $"from {nameof(Type)} "
            + $"decorated with {nameof(ActorAttribute)}"
    )]
    public void Test3()
    {
        // Arrange
        var type = typeof(NoStateActor);

        // Act
        var sut = type.ToActor();

        // Assert
        using var scope = new AssertionScope();
        sut.Should().BeAssignableTo<IActor>();
        sut.States.Should().HaveCount(0);
    }

    [Fact(
        DisplayName = $"Cannot create {nameof(IActor)} "
            + $"with {nameof(IState)} having multiple constructors "
            + $"but not decorated with {nameof(StateConstructorAttribute)}"
    )]
    public void Test4()
    {
        // Arrange
        var type = typeof(StateWithManyConstructorWithoutAttributeActor);

        // Act
        Action sut = () => type.ToActor();

        // Assert
        using var scope = new AssertionScope();
        sut.Should().ThrowExactly<MultipleStateConstructorsException>();
    }

    [Fact(
        DisplayName = $"Cannot create {nameof(IActor)} "
            + $"with {nameof(IState)} having multiple constructors "
            + $"decorated with {nameof(StateConstructorAttribute)}"
    )]
    public void Test5()
    {
        // Arrange
        var type = typeof(StateWithManyConstructorWithAttributeActor);

        // Act
        Action sut = () => type.ToActor();

        // Assert
        using var scope = new AssertionScope();
        sut.Should().ThrowExactly<MultipleStateConstructorsException>();
    }

    [Fact(
        DisplayName = $"Cannot create {nameof(IActor)} "
            + $"not decorated with {nameof(StateConstructorAttribute)}"
    )]
    public void Test6()
    {
        // Arrange
        var type = typeof(object);

        // Act
        Action sut = () => type.ToActor();

        // Assert
        using var scope = new AssertionScope();
        sut.Should().ThrowExactly<TypeIsNotActorException>();
    }
}
