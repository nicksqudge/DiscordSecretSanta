using CSharpFunctionalExtensions;
using FluentAssertions.Primitives;
using Microsoft.VisualBasic.CompilerServices;
using NSubstitute;

namespace DiscordSecretSanta.Core.Tests.TestHelpers.DepedencyAssertions;

public class AuthProviderServiceHelper : DependencyHelper<IAuthProviderService, AuthProviderAssertions>
{
    protected override AuthProviderAssertions _assertions => new (Object);

    public void ReturnsGetCurrentUser(AuthenticatedUser user)
    {
        Object.GetCurrentUser(default)
            .ReturnsForAnyArgs(user);
    }

    public void ReturnsGetCurrentUser()
    {
        ReturnsGetCurrentUser(new AuthenticatedUser("Test", "1234", "1234", new UserId("1234")));
    }

    public void ReturnsNoCurrentUser()
    {
        Object.GetCurrentUser(default)
            .ReturnsForAnyArgs(Maybe<AuthenticatedUser>.None);
    }
}

public class AuthProviderAssertions : ReferenceTypeAssertions<IAuthProviderService, AuthProviderAssertions>
{
    public AuthProviderAssertions(IAuthProviderService subject) : base(subject)
    {
    }

    protected override string Identifier => nameof(AuthProviderAssertions);
}