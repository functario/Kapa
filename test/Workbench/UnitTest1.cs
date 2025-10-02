using AwesomeAssertions.Execution;
using Kapa.Abstractions.Capabilities;
using Kapa.Core.Extensions;
using Newtonsoft.Json;

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

        var a = JsonConvert.SerializeObject(kapaB);

        kapaA.Should().NotBeNull().And.Satisfy<IKapability>(k => k.Steps.Should().HaveCount(1));
        kapaB.Should().NotBeNull().And.Satisfy<IKapability>(k => k.Steps.Should().HaveCount(1));
    }
}
