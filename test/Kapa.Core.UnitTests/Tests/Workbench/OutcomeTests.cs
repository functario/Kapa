//using System.Text.Json;
//using System.Text.Json.Serialization;
//using Kapa.Abstractions.Validations;
//using Kapa.Core.Validations;
//using Kapa.Fixtures.Capabilities.WithTypedOutcomes;

//namespace Kapa.Core.UnitTests.Tests.Workbench;

//public class OutcomeTests
//{
//    [Fact]
//    public void Test1()
//    {
//        // Arrange
//        var capa = new OkOutcomeCapability();

//        // Act
//        var sut = capa.Handle();

//        // Assert
//        sut.Should().BeAssignableTo<IOutcome>();
//        sut.Status.Should().Be(OutcomeStatus.Ok);
//    }

//    [Fact]
//    public void Test2()
//    {
//        // Arrange
//        var capa = new OkOutcomeCapability();

//        // Act
//        var sut = capa.Handle();

//        // Assert
//        sut.Should().BeAssignableTo<IOutcome>();
//        sut.Status.Should().Be(OutcomeStatus.Ok);
//    }

//    [Theory]
//    [InlineData(OutcomeStatus.Ok, typeof(Ok<string>))]
//    [InlineData(OutcomeStatus.Fail, typeof(Fail<string>))]
//    [InlineData(OutcomeStatus.RulesFail, typeof(RulesFail<string>))]
//    public void Test3(OutcomeStatus outcomeStatus, Type innerType)
//    {
//        // Arrange
//        var capa = new OkStrOrFailStrOrRulesFailStrOutcomeCapability(outcomeStatus);

//        // Act
//        var sut = capa.Handle();

//        // Assert
//        sut.Should().BeAssignableTo<IOutcome>();
//        sut.Should().BeOfType<Outcomes<Ok<string>, Fail<string>, RulesFail<string>>>();
//        sut.Outcome.Should().BeAssignableTo(innerType);
//    }

//    [Fact]
//    public void Test4()
//    {
//        // Arrange
//        var capa = new OkOfTCapability();

//        // Act
//        var sut = capa.Handle();

//        // Assert
//        sut.Should().BeAssignableTo<IOutcome>();
//        sut.Status.Should().Be(OutcomeStatus.Ok);
//    }

//    [Fact(
//        DisplayName = $"Infer {nameof(IOutcomeMetadata)} from {nameof(ICapability)} creation "
//            + $"{nameof(OkStrOrFailStrOrRulesFailStrOutcomeCapability)}"
//    )]
//    public void Test5()
//    {
//        // Arrange
//        var capabilityType = new OkStrOrFailStrOrRulesFailStrOutcomeCapability(OutcomeStatus.Ok);
//        var type = typeof(OkStrOrFailStrOrRulesFailStrOutcomeCapability);

//        // Act
//        var capability = type.GetCapabilities();
//        var outcome = capabilityType.Handle();

//        // Assert
//#pragma warning disable CA1869 // Cache and reuse 'JsonSerializerOptions' instances
//        var options = new JsonSerializerOptions
//        {
//            WriteIndented = true,
//            // Allow to transform \u003E to <> for generic source
//            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
//            Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) },
//        };
//#pragma warning restore CA1869 // Cache and reuse 'JsonSerializerOptions' instances
//        var a = JsonSerializer.Serialize(capability, options);
//        var b = JsonSerializer.Serialize(outcome, options);
//    }

//    [Fact(
//        DisplayName = $"Infer {nameof(IOutcomeMetadata)} from {nameof(ICapability)} creation. "
//            + $"{nameof(OkOrFailOutcomeCapability)}"
//    )]
//    public void Test6()
//    {
//        // Arrange
//        var capabilityType = new OkOrFailOutcomeCapability(true);
//        var type = typeof(OkOrFailOutcomeCapability);

//        // Act
//        var capability = type.GetCapabilities();
//        var outcome = capabilityType.Handle();

//        // Assert
//#pragma warning disable CA1869 // Cache and reuse 'JsonSerializerOptions' instances
//        var options = new JsonSerializerOptions
//        {
//            WriteIndented = true,
//            // Allow to transform \u003E to <> for generic source
//            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
//            Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) },
//        };
//#pragma warning restore CA1869 // Cache and reuse 'JsonSerializerOptions' instances

//        // Note: The outcome will not have the same flags than the Capabilities.
//        // Because when the outcome is returned from Handle(),
//        // the union is resolved to the underlying Outcome.
//        var a = JsonSerializer.Serialize(capability, options);
//        var b = JsonSerializer.Serialize(outcome, options);
//    }
//}
