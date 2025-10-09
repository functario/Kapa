using System.Reflection;
using HomeAutomation.Prototypes;
using HomeAutomation.Prototypes.Identifications;
using Kapa.Core.Capabilities;
using Kapa.Core.Validations;

namespace HomeAutomation.Capabilities;

[CapabilityType]
public sealed class AuthenticationCapabilities
{
    private readonly IUser _user;
    private readonly TimeProvider _timeProvider;

    public AuthenticationCapabilities(IUser user, TimeProvider timeProvider)
    {
        _user = user;
        _timeProvider = timeProvider;
    }

    [Capability($"Authenticate the {nameof(User)}.")]
    public async Task<Outcomes<Ok<IUser>, Fail<string>>> AuthenticateAsync(
        string userName,
        string password
    )
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(userName, nameof(userName));
        ArgumentException.ThrowIfNullOrWhiteSpace(password, nameof(password));

        // Busy task
        await Task.Delay(10);
        var token = new Token(
            Guid.NewGuid().ToString(),
            Guid.NewGuid().ToString(),
            _timeProvider.GetUtcNow().AddHours(1)
        );

        _user.Identification = new Identification() { IsAuthenticated = true, Token = token };

        // example for handling failure
        if (userName is null)
        {
            return TypedOutcomes.Fail<string>(
                MethodInfo.GetCurrentMethod(),
                $"{nameof(User)} is authenticated."
            );
        }

        return TypedOutcomes.Ok(MethodInfo.GetCurrentMethod(), _user);
    }
}
