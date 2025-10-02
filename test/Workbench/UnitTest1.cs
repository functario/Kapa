using AwesomeAssertions.Execution;

namespace Workbench;

public class UnitTest1
{
    [Fact]
    public void Test1()
    {
        // Act
        var kapaA = new KapaA();
        var kapaB = new KapaB();

        // Assert
        using var scope = new AssertionScope();
        kapaA.Should().NotBeNull();
        kapaB.Should().NotBeNull();
    }
}
