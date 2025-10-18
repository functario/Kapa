using System.Reflection;
using Kapa.Core.Extensions;

namespace HomeAutomation.Capabilities;

[CapabilityType]
public sealed class AuthenticationCapabilities
{
    private readonly TimeProvider _timeProvider;

    public AuthenticationCapabilities(TimeProvider timeProvider)
    {
        _timeProvider = timeProvider;
    }

    [Capability($"Authenticate the {nameof(User)}.")]
    [Relations<AuthenticationsRelations>]
    public async Task<Outcomes<Ok<IUser>, Fail<string>>> AuthenticateAsync(
        IUser user,
        [Parameter($"The {nameof(User)} email used for authentification.")] string email,
        [Parameter($"The {nameof(User)} password used for authentification.")] string password,
        CancellationToken cancellationToken
    )
    {
        ArgumentNullException.ThrowIfNull(user);
        ArgumentException.ThrowIfNullOrWhiteSpace(email, nameof(email));
        ArgumentException.ThrowIfNullOrWhiteSpace(password, nameof(password));

        // Busy task
        await Task.Delay(10, cancellationToken);
        var token = Token.CreateDummy();

        user.Identification = new Identification() { IsAuthenticated = true, Token = token };

        // example for handling failure
        if (email is null)
        {
            return TypedOutcomes.Fail<string>(
                MethodInfo.GetCurrentMethod(),
                $"{nameof(User)} is authenticated."
            );
        }

        return TypedOutcomes.Ok(MethodInfo.GetCurrentMethod(), user);
    }
}

public sealed class AuthenticationsRelations : IRelations<IGeneratedActor>
{
    public ICollection<IEffect<IGeneratedActor>> Mutations =>
        // For example: Simulate enforcing the origin of the predicate if not inside the IGeneratedActor
        [IUser.IsAuthenticated.ToEffect<IUser, IUser>()];

    public ICollection<IEffect<IGeneratedActor>> Requirements => [];
}
