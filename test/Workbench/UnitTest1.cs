using AwesomeAssertions.Execution;
using Kapa.Abstractions.Capabilities;
using Kapa.Core.Extensions;

namespace Workbench;

public class UnitTest1
{
    [Fact]
    public void Test1()
    {
        // Act
        var kapaA = typeof(KapaA).ToKapability();
        var kapaB = typeof(KapaB).ToKapability();

        // Assert
        using var scope = new AssertionScope();
        kapaA.Should().NotBeNull().And.Satisfy<IKapability>(k => k.KapaSteps.Should().HaveCount(1));
        kapaB.Should().NotBeNull().And.Satisfy<IKapability>(k => k.KapaSteps.Should().HaveCount(1));
    }
}
