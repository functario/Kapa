using Kapa.Fixtures.States;

namespace Kapa.Core.UnitTests.Tests.Extensions;

public class StateTypeExtensionsTests
{
    [Fact]
    public void MyTestMethod()
    {
        // Arrange
        var a = typeof(StateWithOneTrait).ToState();
        // Act

        // Assert
    }
}
