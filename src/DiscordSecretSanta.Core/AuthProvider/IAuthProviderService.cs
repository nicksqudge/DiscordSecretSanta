using CSharpFunctionalExtensions;

namespace DiscordSecretSanta.Core.AuthProvider;

public interface IAuthProviderService
{
    Task<Maybe<AuthenticatedUser>> GetCurrentUser(CancellationToken cancellationToken);
}