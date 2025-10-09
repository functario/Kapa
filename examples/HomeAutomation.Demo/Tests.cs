using AwesomeAssertions;
using AwesomeAssertions.Execution;
using HomeAutomation.Actors;
using HomeAutomation.Actors.Homes;
using HomeAutomation.Capabilities;
using Kapa.Abstractions.Graphs;
using Kapa.Core.Extensions;
using Kapa.Core.Validations;
using Microsoft.Testing.Platform.Capabilities;

namespace HomeAutomation.Demo;

public class Tests
{
    [Fact(DisplayName = $"{nameof(User)} is a shared instance between {nameof(ICapability)}.")]
    public async Task Test1()
    {
        // Arrange
        var user = new User();
        var setups = new SetupCapabilities(user);
        var timeProvider = TimeProvider.System;
        var authentications = new AuthenticationCapabilities(user, timeProvider);
        var domotic = new DomoticCapabilities(user);
        var tstat00Name = "Thermostat00";
        var expectedSetpoint = 17.5;

        // Act
        setups.Setup();
        var sut1 = await authentications.AuthenticateAsync("user@home.com", "1234!");
        var sut2 = await domotic.SetThermostatSetpoint(tstat00Name, expectedSetpoint);

        // Assert
        using var scope = new AssertionScope();
        var userOutcome1 = sut1.Outcome.As<Ok<IUser>>().Value;
        var userOutcome2 = sut2.Outcome.As<Ok<IUser>>().Value;

        // The same user all along.
        user.Should().BeSameAs(userOutcome1).And.BeSameAs(userOutcome2).And.NotBe(null);

        // Validate Thermostat changes.
        var tstat00 = user.Home?.GetDevice<Thermostat>(tstat00Name);
        tstat00.Should().NotBeNull();
        tstat00.Setpoint.Should().Be(expectedSetpoint);

        // Validate Authentication.
        var identification = sut1.Outcome.As<Ok<IUser>>().Value?.Identification;
        identification.Should().NotBeNull();
        identification!.IsAuthenticated.Should().BeTrue();
        identification!.Token?.AccessToken.Should().NotBeNullOrEmpty();
        identification!.Token?.RefreshToken.Should().NotBeNullOrEmpty();
        identification!.Token?.ExpiresOn.Should().BeAfter(timeProvider.GetUtcNow());
    }

    [Fact(DisplayName = $"Resolve {nameof(IGraph)}")]
    public void Test2()
    {
        // Arrange
        var thermostat = new Thermostat(Guid.NewGuid(), "Thermostat00", "ThermostatModel");
        // Act
        var sut = typeof(DomoticCapabilities).GetCapabilities();

        // Assert
    }
}
