using Kapa.Abstractions.Prototypes;
using Kapa.Core.Prototypes;
using Kapa.Fixtures.Prototypes;

namespace Kapa.Core.UnitTests.Tests.Extensions;

public class PrototypeTypeExtensionsTests
{
    [Fact(
        DisplayName = $"Create {nameof(IPrototype)} with one {nameof(ITrait)} "
            + $"from {nameof(Type)} "
            + $"decorated with {nameof(PrototypeAttribute)}"
    )]
    public void Test1()
    {
        // Arrange
        var type = typeof(OneTraitPrototype);

        // Act
        var sut = type.ToPrototype();

        // Assert
        using var scope = new AssertionScope();
        sut.Should().BeAssignableTo<IPrototype>();
        sut.Traits.Should().HaveCount(1);
        sut.Traits.First().Name.Should().Be(nameof(OneTraitPrototype.BoolTrait));
    }

    [Fact(
        DisplayName = $"Create {nameof(IPrototype)} with many {nameof(ITrait)} "
            + $"from {nameof(Type)} "
            + $"decorated with {nameof(PrototypeAttribute)}"
    )]
    public void Test2()
    {
        // Arrange
        var type = typeof(ManyTraitsPrototype);

        // Act
        var sut = type.ToPrototype();

        // Assert
        using var scope = new AssertionScope();
        sut.Should().BeAssignableTo<IPrototype>();
        sut.Traits.Should().HaveCount(4);
        sut.Traits.Select(x => x.Name)
            .Should()
            .BeEquivalentTo(
                [
                    nameof(ManyTraitsPrototype.BoolTrait),
                    nameof(ManyTraitsPrototype.RecordTrait),
                    nameof(ManyTraitsPrototype.ClassPrimaryConstructorTrait),
                    nameof(ManyTraitsPrototype.ClassMultiConstructorsTrait),
                ]
            );
    }

    [Fact(
        DisplayName = $"Create {nameof(IPrototype)} without {nameof(ITrait)} "
            + $"from {nameof(Type)} "
            + $"decorated with {nameof(PrototypeAttribute)}"
    )]
    public void Test3()
    {
        // Arrange
        var type = typeof(NoTraitPrototype);

        // Act
        var sut = type.ToPrototype();

        // Assert
        using var scope = new AssertionScope();
        sut.Should().BeAssignableTo<IPrototype>();
        sut.Traits.Should().HaveCount(0);
    }

    [Fact(
        DisplayName = $"Cannot create {nameof(IPrototype)} "
            + $"with {nameof(ITrait)} having multiple constructors "
            + $"but not decorated with {nameof(TraitConstructorAttribute)}"
    )]
    public void Test4()
    {
        // Arrange
        var type = typeof(TraitWithManyConstructorWithoutAttributePrototype);

        // Act
        Action sut = () => type.ToPrototype();

        // Assert
        using var scope = new AssertionScope();
        sut.Should().ThrowExactly<MultipleTraitConstructorsException>();
    }

    [Fact(
        DisplayName = $"Cannot create {nameof(IPrototype)} "
            + $"with {nameof(ITrait)} having multiple constructors "
            + $"decorated with {nameof(TraitConstructorAttribute)}"
    )]
    public void Test5()
    {
        // Arrange
        var type = typeof(TraitWithManyConstructorWithAttributePrototype);

        // Act
        Action sut = () => type.ToPrototype();

        // Assert
        using var scope = new AssertionScope();
        sut.Should().ThrowExactly<MultipleTraitConstructorsException>();
    }

    [Fact(
        DisplayName = $"Cannot create {nameof(IPrototype)} "
            + $"not decorated with {nameof(TraitConstructorAttribute)}"
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
